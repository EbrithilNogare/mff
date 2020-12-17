/** @file

ckir.hpp

A wrapper over LLVM IR.

*/

#ifndef ckir_hpp_
#define ckir_hpp_

#include "llvm/ADT/APInt.h"
#include "llvm/IR/Verifier.h"
#include "llvm/ExecutionEngine/ExecutionEngine.h"
#include "llvm/ExecutionEngine/GenericValue.h"
#include "llvm/ExecutionEngine/MCJIT.h"
#include "llvm/IR/Argument.h"
#include "llvm/IR/BasicBlock.h"
#include "llvm/IR/Constants.h"
#include "llvm/IR/DerivedTypes.h"
#include "llvm/IR/Function.h"
#include "llvm/IR/InstrTypes.h"
#include "llvm/IR/Instructions.h"
#include "llvm/IR/LLVMContext.h"
#include "llvm/IR/Module.h"
#include "llvm/IR/Type.h"
#include "llvm/IR/IRBuilder.h"
#include "llvm/Support/Casting.h"
#include "llvm/Support/TargetSelect.h"
#include "llvm/Support/raw_ostream.h"
#include "llvm/Bitcode/BitcodeWriter.h"
#include "llvm/Support/raw_os_ostream.h"

#include <ostream>
#include <cstdio>

namespace cecko {

	/// @cond INTERNAL
	template< typename E>
	struct safe_default
	{
		E operator()() const { return E(); }
	};
	/// @endcond

	/// @brief Safe pointer to E
	/// 
	/// No crashes: When the value is nullptr, * and -> returns a dummy object responding to all member functions.
	/// No random values: Initialized to nullptr.
	/// Automatic conversion to E*.
	/// Explicit conversion from E*.
	template< typename E, typename DF = safe_default<E>>
	class safe_ptr
	{
	public:
		safe_ptr() : p_(nullptr) {}
		safe_ptr(std::nullptr_t) : p_(nullptr) {}
		template<typename E2, typename DF2, std::enable_if_t<std::is_convertible_v<E2*,E*>,bool> = true>
		safe_ptr(const safe_ptr<E2, DF2>& b) : p_(b.p_) {}
		explicit safe_ptr(E* p) : p_(p) {}
		operator E* () const { return p_; }
		operator bool() const { return !!p_; }
		E& operator*() const { return p_ ? *p_ : dummy(); }
		E* operator->() const { return p_ ? p_ : &dummy(); }
		friend bool operator==(const safe_ptr& a, const safe_ptr& b) { return a.p_ == b.p_; }
		friend bool operator!=(const safe_ptr& a, const safe_ptr& b) { return a.p_ != b.p_; }
	private:
		E* p_;
		static E& dummy() { static decltype(DF()()) d = DF()(); return d; }
		template<typename E2, typename DF2>
		friend class safe_ptr;
	};

	// numbers
	using CKIRAPInt = llvm::APInt;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1APInt.html">llvm::APInt</a>
	// context
	using CKIRContextRef = llvm::LLVMContext&;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1LLVMContext.html">llvm::LLVMContext</a>
	// types
	using CKIRTypeObs = llvm::Type*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1Type.html">llvm::Type</a>
	using CKIRStructTypeObs = llvm::StructType*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1StructType.html">llvm::StructType</a>
	using CKIRFunctionTypeObs = llvm::FunctionType*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1FunctionType.html">llvm::FunctionType</a>
	using CKIRTypeObsArray = std::vector<llvm::Type*>;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1Type.html">llvm::Type</a>
	using CKIRTypeObsArrayRef = llvm::ArrayRef<llvm::Type*>;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1ArrayRef.html">llvm::ArrayRef</a> @sa <a href="http://llvm.org/doxygen/classllvm_1_1Type.html">llvm::Type</a>
	// values
	using CKIRValueObs = llvm::Value*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1Value.html">llvm::Value</a>
	using CKIRValueObsArray = std::vector<llvm::Value*>;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1Value.html">llvm::Value</a>
	using CKIRValueObsArrayRef = llvm::ArrayRef<llvm::Value*>;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1ArrayRef.html">llvm::ArrayRef</a> @sa <a href="http://llvm.org/doxygen/classllvm_1_1Value.html">llvm::Value</a>
	// constant values
	using CKIRConstantObs = llvm::Constant*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1Constant.html">llvm::Constant</a>
	using CKIRConstantIntObs = llvm::ConstantInt*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1ConstantInt.html">llvm::ConstantInt</a>
	// module
	using CKIRModuleObs = llvm::Module*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1Module.html">llvm::Module</a>
	// function
	using CKIRFunctionObs = llvm::Function*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1Function.html">llvm::Function</a>
	// basic block
	using CKIRBasicBlockObs = llvm::BasicBlock*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1BasicBlock.html">llvm::BasicBlock</a>
	// instructions
	using CKIRAllocaInstObs = llvm::AllocaInst*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1AllocaInst.html">llvm::AllocaInst</a>
	// builder
	using CKIRBuilder = llvm::IRBuilder<>;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1IRBuilder.html">llvm::IRBuilder</a>
	using CKIRBuilderRef = CKIRBuilder&;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1IRBuilder.html">llvm::IRBuilder</a>
	using CKIRBuilderObs = CKIRBuilder*;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1IRBuilder.html">llvm::IRBuilder</a>

	// string
	using CKIRName = llvm::Twine;	///< @sa <a href="http://llvm.org/doxygen/classllvm_1_1Twine.html">llvm::Twine</a>

	/// @cond INTERNAL

	inline std::size_t CKHashValue(const CKIRAPInt& Arg)
	{
		return llvm::hash_value(Arg);
	}

	inline CKIRTypeObs CKGetVoidType(CKIRContextRef Context)
	{
		return llvm::Type::getVoidTy(Context);
	}

	inline CKIRTypeObs CKGetInt1Type(CKIRContextRef Context)
	{
		return llvm::Type::getInt1Ty(Context);
	}

	inline CKIRTypeObs CKGetInt8Type(CKIRContextRef Context)
	{
		return llvm::Type::getInt8Ty(Context);
	}

	inline CKIRTypeObs CKGetInt32Type(CKIRContextRef Context)
	{
		return llvm::Type::getInt32Ty(Context);
	}

	inline CKIRTypeObs CKGetInt8PtrType(CKIRContextRef Context)
	{
		return llvm::Type::getInt8PtrTy(Context);
	}

	CKIRTypeObs CKGetPtrType(CKIRTypeObs element);

	CKIRTypeObs CKGetArrayType(CKIRTypeObs element, CKIRConstantIntObs size);

	inline CKIRStructTypeObs CKCreateStructType(CKIRContextRef Context, const std::string& name)
	{
		return llvm::StructType::create(Context, name);
	}

	inline CKIRFunctionTypeObs CKGetFunctionType(CKIRTypeObs rettype, CKIRTypeObsArrayRef argtypes, bool variadic = false)
	{
		return llvm::FunctionType::get(rettype, argtypes, variadic);
	}

	inline CKIRConstantIntObs CKGetInt1Constant(CKIRContextRef Context, bool V)
	{
		return llvm::ConstantInt::get(llvm::Type::getInt1Ty(Context), V);
	}
		
	inline CKIRConstantIntObs CKGetInt8Constant(CKIRContextRef Context, std::int_fast8_t V)
	{
		return llvm::ConstantInt::get(llvm::Type::getInt8Ty(Context), V);
	}

	inline CKIRConstantIntObs CKGetInt32Constant(CKIRContextRef Context, std::int_fast32_t V)
	{
		return llvm::ConstantInt::get(llvm::Type::getInt32Ty(Context), V);
	}

	inline CKIRFunctionObs CKCreateFunction(CKIRFunctionTypeObs FT, const std::string& name, CKIRModuleObs M)
	{
		return llvm::Function::Create(FT, llvm::Function::ExternalLinkage, name, M);
	}

	/// Create a new basic block
	inline CKIRBasicBlockObs CKCreateBasicBlock(const std::string& name, CKIRFunctionObs F)
	{
		if (!F)
			return nullptr;
		return llvm::BasicBlock::Create(F->getContext(), name, F);
	}
	/// @endcond

	/// Get integral constant from a llvm::Value
	inline CKIRConstantIntObs CKTryGetConstantInt(CKIRValueObs v)
	{
		if (!v)
			return nullptr;
		if (!llvm::isa< llvm::ConstantInt>(v))
			return nullptr;
		return llvm::cast<llvm::ConstantInt>(v);
	}

	/// Get null value for a llvm::Type
	inline CKIRConstantObs CKIRNullValue(CKIRTypeObs t)
	{
		return llvm::Constant::getNullValue(t);
	}

	/// @cond INTERNAL
	CKIRConstantObs CKCreateGlobalVariable(CKIRTypeObs irtp, const std::string& name, CKIRModuleObs M);

	CKIRConstantObs CKCreateExternVariable(CKIRTypeObs irtp, const std::string& name, CKIRModuleObs M);

	using CKIRDataLayoutObs = const llvm::DataLayout*;

	inline std::int_fast64_t CKGetTypeSize(CKIRDataLayoutObs DataLayout, CKIRTypeObs Ty)
	{
		auto ts = DataLayout->getTypeAllocSize(Ty);
		return ts;
	}

	class CKIREnvironment {
	public:
		CKIREnvironment();

		void dump_module(std::ostream& os, CKIRModuleObs module) const;

		std::error_code write_bitcode_module(const std::string& fname, CKIRModuleObs module) const;

		int run_main(CKIRFunctionObs fnc, int argc, char** argv, std::ostream& os);
		
		CKIRContextRef context()
		{
			return *ckircontextptr_;
		}

		CKIRModuleObs module()
		{
			return ckirmoduleobs_;
		}

		CKIRDataLayoutObs data_layout() const
		{
			return &*ckirdatalayoutptr_;
		}

	private:
		std::unique_ptr< llvm::LLVMContext> ckircontextptr_;
		std::unique_ptr< llvm::Module> ckirmoduleptr_;
		CKIRModuleObs ckirmoduleobs_;
		std::unique_ptr< llvm::DataLayout> ckirdatalayoutptr_;
	};

	using CKIREnvironmentObs = CKIREnvironment*;
	/// @endcond
};

#endif 
