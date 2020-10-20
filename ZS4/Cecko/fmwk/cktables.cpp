#include "cktables.hpp"

namespace cecko {
	// DECLARATION GENERATOR

	CIDecl decl_const(bool is_const)
	{
		return is_const ? "const " : "";
	}

	CIDecl decl_dtor(bool no_space, bool in_suffix, const CIDecl& dtor)
	{
		return in_suffix && !dtor.empty() && dtor[0] == '*'
			? "(" + dtor + ")"
			: (no_space || dtor.empty() || dtor[0] == '*' || dtor[0] == '('
				? dtor
				: " " + dtor);
	}

	CIDecl CKVoidType::declaration(bool is_const, const CIDecl& dtor) const { return decl_const(is_const) + "void" + decl_dtor(false, false, dtor); }

	CIDecl CKBoolType::declaration(bool is_const, const CIDecl& dtor) const { return decl_const(is_const) + "bool" + decl_dtor(false, false, dtor); }

	CIDecl CKCharType::declaration(bool is_const, const CIDecl& dtor) const { return decl_const(is_const) + "char" + decl_dtor(false, false, dtor); }

	CIDecl CKIntType::declaration(bool is_const, const CIDecl& dtor) const { return decl_const(is_const) + "int" + decl_dtor(false, false, dtor); }

	CIDecl CKPtrType::declaration(bool is_const, const CIDecl& dtor) const { return points_to_.type->declaration(points_to_.is_const, "*" + (is_const ? "const" + decl_dtor(false, false, dtor) : decl_dtor(true, false, dtor))); }

	CIDecl CKArrayType::declaration(bool is_const, const CIDecl& dtor) const { return element_type_->declaration(is_const, decl_dtor(true, true, dtor) + "[" + std::to_string(size_->getValue().getZExtValue()) + "]"); }

	CIDecl CKStructType::declaration(bool is_const, const CIDecl& dtor) const { return decl_const(is_const) + "struct " + get_name() + decl_dtor(false, false, dtor); }

	void CKStructType::finalize(const CKStructItemArray& items)
	{
		assert(!defined_);
		CKIRTypeObsArray elements_ir;
		unsigned int idx = 0;
		for (auto&& a : items)
		{
			elements_ir.push_back(a.pack.type->get_ir());
			auto p = elements_.try_emplace(a.name, a.pack, idx);
			elements_ordered_.push_back(p);
			++idx;
		}
		irt_->setBody(elements_ir);
		defined_ = true;
	}

	void CKStructType::dump(CIOStream& os) const
	{
		os << "struct " << get_name() << "{" << CIEndl;
		for (auto&& a : elements_ordered_)
			os << "\t" << a->get_type_pack().type->declaration(a->get_type_pack().is_const, a->get_name()) << ";" << CIEndl;
		os << "};" << CIEndl;
	}

	CKFunctionType::CKFunctionType(CKTypeObs ret_type, CKTypeObsArray arg_types, bool variadic)
		: ret_type_(ret_type), arg_types_(std::move(arg_types)), variadic_(variadic), irt_(nullptr)
	{
		CKIRTypeObsArray arg_ir_types(arg_types_.size());
		std::transform(arg_types_.begin(), arg_types_.end(), arg_ir_types.begin(), [](auto&& a) { return a->get_ir(); });
		irt_ = CKGetFunctionType(ret_type_->get_ir(), std::move(arg_ir_types), variadic_);
	}

	bool CKFunctionType::operator==(const CKFunctionType& b) const
	{
		bool eq = ret_type_ == b.ret_type_
			&& arg_types_.size() == b.arg_types_.size()
			&& variadic_ == b.variadic_;
		for (auto i = std::size_t(0); eq && i < arg_types_.size(); ++i)
			eq = eq && arg_types_[i] == b.arg_types_[i];
		return eq;
	}

	std::size_t CKFunctionType::hash() const
	{
		auto h = ret_type_->hash() ^ compute_hash(arg_types_.size(), variadic_);
		for (auto&& a : arg_types_)
			h ^= a->hash();
		return h;
	}

	CITypeMangle CKFunctionType::mangle() const
	{
		auto m = (variadic_ ? "fv" : "f") + ret_type_->mangle();
		for (auto&& a : arg_types_)
			m += "_" + a->mangle();
		return m;
	}
	CIDecl CKFunctionType::declaration(bool is_const, const CIDecl& dtor) const {
		CIDecl ad = "";
		for (auto&& a : arg_types_)
		{
			if (!ad.empty()) ad += ",";
			ad += a->declaration(false, "");
		}
		if (variadic_)
		{
			if (!ad.empty()) ad += ",";
			ad += "...";
		}
		return ret_type_->declaration(false, decl_dtor(true, true, dtor) + "(" + ad + ")");
	}

	void CKTypeTable::dump(CIOStream& os) const
	{
		/*
		auto dlambda = [&os](auto&& a) {
			os << "typedef " << a->declaration(false, a->mangle()) << ";" << CIEndl;
		};
		strts_.for_each(dlambda);
		dlambda(&boot_);
		dlambda(&chrt_);
		dlambda(&intt_);
		ptrts_.for_each(dlambda);
		arrts_.for_each(dlambda);
		fncts_.for_each(dlambda);
		*/
		strts_.for_each([&os](auto&& a) {
			a->dump(os);
			});
	}

	CKGlobalVarObs CKGlobalTable::varDefine(CKIRModuleObs M, const std::string& name, const CKTypeRefPack& type_pack)
	{
		auto irtp = type_pack.type->get_ir();
		auto var = CKCreateGlobalVariable(irtp, name, M);
		return vars_.try_emplace(name, type_pack, var);
	}
	CKGlobalVarObs CKGlobalTable::declare_extern_variable(CKIRModuleObs M, const std::string& name, const CKTypeRefPack& type_pack)
	{
		auto irtp = type_pack.type->get_ir();
		auto var = CKCreateExternVariable(irtp, name, M);
		return vars_.try_emplace(name, type_pack, var);
	}
	CKFunctionObs CKGlobalTable::declare_function(const CIName& n, CKIRModuleObs M, CKFunctionTypeObs type)
	{
		return fncs_.try_emplace(n, M, type, n);
	}
	CKFunctionObs CKGlobalTable::declare_function(const CIName& n, CKIRModuleObs M, CKFunctionTypeObs type, const std::string& irname)
	{
		return fncs_.try_emplace(n, M, type, irname);
	}
	CKFunctionObs CKGlobalTable::find_function(const CIName& n)
	{
		auto p = fncs_.find(n);
		return p;
	}
	CKFunctionConstObs CKGlobalTable::find_function(const CIName& n) const
	{
		auto p = fncs_.find(n);
		return p;
	}
	CKNamedObs CKGlobalTable::find(const CIName& n)
	{
		auto q = vars_.find(n);
		if (!!q)
			return q;
		auto p = fncs_.find(n);
		return p;
	}

	void CKGlobalTable::dump(CIOStream& os) const
	{
		auto decllambda = [&os](auto&& a) {
			os << a->get_type()->declaration(false, a->get_name()) << ";" << CIEndl;
		};
		fncs_.for_each(decllambda);
		auto varlambda = [&os](auto&& a) {
			os << a->get_type()->declaration(false, a->get_name()) << ";" << CIEndl;
		};
		vars_.for_each(varlambda);
		auto deflambda = [&os](auto&& a) {
			if (a->is_defined())
			{
				a->dump(os);
			}
		};
		fncs_.for_each(deflambda);
	}

	void CKLocalTable::varsFromArgs(CKIRBuilderRef builder, CKFunctionObs f, const CKFunctionFormalPackArray& formal_packs)
	{
		function_ = f;
		auto f_type = f->get_function_type();
		auto f_ir = f->get_function_ir();
		auto n = f_type->get_function_arg_count();
		assert(n == formal_packs.size());
		assert(n == f_ir->getFunctionType()->getNumParams());
		for (std::size_t ix = 0; ix < n; ++ix)
		{
			auto&& arg_pack = formal_packs[ix];
			if (!!arg_pack.name)	// unnamed arguments are not accessible inside the function
			{
				auto arg_type = f_type->get_function_arg_type(ix);
				auto arg_ir = f_ir->args().begin() + ix;
				auto var = builder.CreateAlloca(arg_type->get_ir(), nullptr, *arg_pack.name);
				builder.CreateStore(arg_ir, var);
				vars_.try_emplace(*arg_pack.name, CKTypeRefPack{ arg_type, arg_pack.is_const }, var, true);
			}
		}
	}

	CKLocalVarObs CKLocalTable::varDefine(CKIRBuilderRef builder, const std::string& name, const CKTypeRefPack& type_pack)
	{
		auto var = builder.CreateAlloca(type_pack.type->get_ir(), nullptr, name);
		return vars_.try_emplace(name, type_pack, var, false);
	}

	CKNamedObs CKLocalTable::find(const CIName& n)
	{
		auto p = vars_.find(n);
		if (p)
			return p;
		return parent_scope_->find(n);
	}
	void CKLocalTable::dump(CIOStream& os) const
	{
		auto dlambda = [&os](auto&& a) {
			if (!a->is_arg())
			{
				a->dump(os);
			}
		};
		vars_.for_each(dlambda);
	}

	CKLocalTableObs CKFunction::define(CKAbstractScopeObs parent, CKIRBuilderRef builder, CKFunctionFormalPackArray formal_packs)
	{
		assert(!loctab_);
		loctab_ = std::make_unique<CKLocalTable>(parent);
		formal_packs_ = std::move(formal_packs);
		loctab_->varsFromArgs(builder, this, formal_packs_);
		return &*loctab_;
	}

	void CKFunction::dump(CIOStream& os) const
	{
		auto f_type = get_function_type();
		std::string args;
		{
			auto n = f_type->get_function_arg_count();
			for (std::size_t ix = 0; ix < n; ++ix)
			{
				auto arg_type = f_type->get_function_arg_type(ix);
				auto&& arg_pack = get_formal_pack(ix);
				if (!args.empty())
					args += ",";
				args += arg_type->declaration(arg_pack.is_const, !!arg_pack.name ? *arg_pack.name : std::string{});
			}
		}
		os << f_type->get_function_return_type()->declaration(false, get_name() + "(" + args + ")") << "{" << CIEndl;

		loctab_->dump(os);

		os << "}" << CIEndl;
	}

	void CKVar::dump(CIOStream& os) const
	{
		os << "\t" << get_type_pack().type->declaration(get_type_pack().is_const, get_name()) << ";" << CIEndl;
	}

	// CONTEXT

	CKTables::CKTables(CKIREnvironmentObs irenv)
		: irenv_(irenv),
		typetable_(irenv->context()),
		globtable_(),
		module_(irenv->module()),
		data_layout_(irenv->data_layout())
	{
		declare_library();
	}

	void CKTables::dump_tables(std::ostream& os) const
	{
		typetable_.dump(os);
		globtable_.dump(os);
	}

	void CKTables::dump_ir_module(std::ostream& os) const
	{
		irenv_->dump_module(os, module_);
	}

	std::error_code CKTables::write_bitcode_module(const std::string& fname) const
	{
		return irenv_->write_bitcode_module(fname, module_);
	}

	CKContext::CKContext(CKTablesObs tab)
		: typetable_(tab->typetable()),
		globtable_(tab->globtable()),
		loctable_(nullptr),
		module_(tab->module()),
		builder_(tab->module()->getContext()),
		data_layout_(tab->data_layout())
	{
	}

	void CKContext::enter_function(CKFunctionObs f, CKFunctionFormalPackArray pack)
	{
		assert(!loctable_);
		// FUNCTION PROLOG
		auto BB = CKCreateBasicBlock("prolog", f->get_function_ir());
		builder_.SetInsertPoint(BB);
		loctable_ = f->define(globtable_, builder_, std::move(pack));
	}

	void CKContext::exit_function()
	{
		assert(loctable_);
		loctable_ = nullptr;
		builder_.ClearInsertionPoint();
	}

	CKVarObs CKContext::define_var(const std::string& name, const CKTypeRefPack& type_pack)
	{
		if (!!loctable_)
		{
			return loctable_->varDefine(builder_, name, type_pack);
		}
		else
		{
			return globtable_->varDefine(module_, name, type_pack);
		}
	}
	CKNamedObs CKContext::find(const CIName& n)
	{
		if (!!loctable_)
		{
			return loctable_->find(n);
		}
		else
		{
			return globtable_->find(n);
		}
	}

	void CKTables::declare_library()
	{
		auto t_void = typetable_.get_void_type();
		auto t_char = typetable_.get_char_type();
		auto t_int = typetable_.get_int_type();
		auto t_ptr_void = typetable_.get_pointer_type({ t_void, false });
		auto t_cptr_void = typetable_.get_pointer_type({ t_void, true });
		auto t_ptr_char = typetable_.get_pointer_type({ t_char, false });
		auto t_cptr_char = typetable_.get_pointer_type({ t_char, true });

		globtable_.declare_function("printf", module_, typetable_.get_function_type(t_int, { t_cptr_char }, true), "ckrt_printf");
		globtable_.declare_function("scanf", module_, typetable_.get_function_type(t_int, { t_cptr_char }, true), "ckrt_scanf");
		globtable_.declare_function("sprintf", module_, typetable_.get_function_type(t_int, { t_ptr_char, t_cptr_char }, true), "ckrt_sprintf");
		globtable_.declare_function("sscanf", module_, typetable_.get_function_type(t_int, { t_cptr_char, t_cptr_char }, true), "ckrt_sscanf");
		globtable_.declare_function("memset", module_, typetable_.get_function_type(t_ptr_void, { t_ptr_void, t_int, t_int }, false), "ckrt_memset");
	}
}
