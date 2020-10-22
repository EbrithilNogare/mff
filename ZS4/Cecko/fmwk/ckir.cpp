#include "ckir.hpp"

#include <cstdio>
#include <cstdarg>

#if defined _WIN32 || defined __CYGWIN__
#ifdef __GNUC__
#define DLL_PUBLIC __attribute__ ((dllexport))
#else
#define DLL_PUBLIC __declspec(dllexport) // Note: actually gcc seems to also supports this syntax.
#endif
#else
#if __GNUC__ >= 4
#define DLL_PUBLIC __attribute__ ((visibility ("default")))
#else
#define DLL_PUBLIC
#endif
#endif

extern "C" {
	int DLL_PUBLIC ckrt_printf(const char* s, ...)
	{
		va_list va;
		va_start(va, s);
		int rv = vprintf(s, va);
		va_end(va);
		fflush(stdout);
		return rv;
	}

	int DLL_PUBLIC ckrt_scanf(const char* s, ...)
	{
		va_list va;
		va_start(va, s);
		int rv = vscanf(s, va);
		va_end(va);
		fflush(stdout);
		return rv;
	}

	int DLL_PUBLIC ckrt_sprintf(char* b, const char* s, ...)
	{
		va_list va;
		va_start(va, s);
		int rv = vsprintf(b, s, va);
		va_end(va);
		fflush(stdout);
		return rv;
	}

	int DLL_PUBLIC ckrt_sscanf(const char* b, const char* s, ...)
	{
		va_list va;
		va_start(va, s);
		int rv = vsscanf(b, s, va);
		va_end(va);
		fflush(stdout);
		return rv;
	}

	void DLL_PUBLIC ckrt_memset(void* d, int s, int l)
	{
		memset(d, s, l);
	}
}

namespace cecko {

	CKIRTypeObs CKGetPtrType(CKIRTypeObs element)
	{
		if (element->isVoidTy())
			return CKGetInt8PtrType(element->getContext());
		return element->getPointerTo();
	}

	CKIRTypeObs CKGetArrayType(CKIRTypeObs element, CKIRConstantIntObs size)
	{
		return llvm::ArrayType::get(element, size->getValue().getSExtValue());
	}

	CKIRConstantObs CKCreateGlobalVariable(CKIRTypeObs irtp, const std::string& name, CKIRModuleObs M)
	{
		auto var = M->getOrInsertGlobal(name, irtp);
		auto gvar = llvm::cast<llvm::GlobalVariable>(var);
		if (irtp->isAggregateType())
			gvar->setInitializer(llvm::ConstantAggregateZero::get(irtp));
		else if (irtp->isPointerTy())
			gvar->setInitializer(llvm::ConstantPointerNull::get(llvm::cast<llvm::PointerType>(irtp)));
		else if (irtp->isIntegerTy())
			gvar->setInitializer(llvm::ConstantInt::get(irtp, 0));
		else
			assert(0);
		return var;
	}

	CKIRConstantObs CKCreateExternVariable(CKIRTypeObs irtp, const std::string& name, CKIRModuleObs M)
	{
		auto var = M->getOrInsertGlobal(name, irtp);
		auto gvar = llvm::cast<llvm::GlobalVariable>(var);
		gvar->setExternallyInitialized(true);
		return var;
	}

	CKIREnvironment::CKIREnvironment()
	{
		llvm::InitializeNativeTarget();
		llvm::InitializeNativeTargetAsmPrinter();
		ckircontextptr_ = std::make_unique<llvm::LLVMContext>();
		ckirmoduleptr_ = std::make_unique<llvm::Module>("test", *ckircontextptr_);
		ckirmoduleobs_ = &*ckirmoduleptr_;
		ckirdatalayoutptr_ = std::make_unique<llvm::DataLayout>(ckirmoduleobs_);
	}

	void CKIREnvironment::dump_module(std::ostream& os, CKIRModuleObs module) const
	{
		llvm::raw_os_ostream raw_os(os);
		raw_os << *module;
	}

	std::error_code CKIREnvironment::write_bitcode_module(const std::string& fname, CKIRModuleObs module) const
	{
		std::error_code oec;
		llvm::raw_fd_ostream ofile(fname, oec);
		if (!oec)
		{
			llvm::WriteBitcodeToFile(*module, ofile);
		}
		return oec;
	}

	int CKIREnvironment::run_main(CKIRFunctionObs fnc, int argc, char** argv)
	{
		//auto&& os = llvm::outs();
		auto&& os = std::cout;
		int mainrv = -1;
		{
			// Now we going to create JIT
			std::string errStr;
			auto EE =
				llvm::EngineBuilder(std::move(ckirmoduleptr_))
				.setErrorStr(&errStr)
				.create();

			if (!EE) {
				os << "========== Failed to construct ExecutionEngine: " << errStr << "==========\n";
				return 1;
			}

			if (verifyModule(*ckirmoduleobs_)) {
				os << "========== Error constructing function ==========\n";
				return 1;
			}

			os << "========== starting main() ==========\n";
			os.flush();	// flush before running unsafe code

			std::vector<llvm::GenericValue> Args(2);
			Args[0].IntVal = llvm::APInt(32, argc);
			Args[1].PointerVal = argv;
			auto GV = EE->runFunction(fnc, Args);
			mainrv = GV.IntVal.getSExtValue();

			// hopefully we destroy JIT here
		}
		os << "\n========== main() returned " << mainrv << " ==========\n";
		return mainrv;
	}
}