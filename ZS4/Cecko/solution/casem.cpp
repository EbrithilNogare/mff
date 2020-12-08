#include "casem.hpp"

namespace casem {
	std::vector<DeclarationSpecifierDto> create_DeclarationSpecifiersDto(){
		return std::vector<DeclarationSpecifierDto>();
	}
	
	std::vector<DeclaratorDto> create_DeclaratorsDto(){
		return std::vector<DeclaratorDto>();
	}

	std::vector<PointerDto> create_PointersDto(){
		return std::vector<PointerDto>();
	}

	cecko::CKTypeObs parse_etype(cecko::context* ctx, cecko::gt_etype etype){
		switch(etype){
			case cecko::gt_etype::BOOL: return ctx->get_bool_type();
			case cecko::gt_etype::CHAR: return ctx->get_char_type();
			case cecko::gt_etype::INT: return ctx->get_int_type();
			default: exit(1); // not implemented
		}
	}

	void DefineVariables(cecko::context* ctx, DeclarationSpecifiersDto specifiers, DeclaratorsDto declarators){
		cecko::CKTypeObs type;
		size_t type_count(0);
		size_t const_count(0);
		size_t typedef_count(0);
		size_t type_ind(0);
		size_t const_ind(0);
		size_t typedef_ind(0);
		bool const_before_type(0);
	
		for (size_t i = 0; i < specifiers.size(); i++)
		{
			if (specifiers[i].is_const){
				const_count++;
				const_ind = i;
			
				if(type_count == 0){
					const_before_type = type;
				}
			}
			if(specifiers[i].is_typedef){
				typedef_count++;
				typedef_ind = i;
			}
			if(specifiers[i].is_type){
				type_count++;
				type_ind = i;
			}
		}

		/* syntax errors */
		// syntax error typedef
		if(typedef_count > 1 || typedef_ind > 0){}
		// syntax too much typest
		if(type_count > 1){}
		// syntax error
		if(const_ind > typedef_ind && const_before_type){}	

		/* define TYPEDEF */
		// todo

		cecko::CKTypeRefPack type_ref(specifiers[type_ind].type, (const_count > 0));

		for (auto&& declarator : declarators)
		{
			for (auto&& pointer : declarator.pointers)
			{
				cecko::CKPtrTypeSafeObs envelope = ctx->get_pointer_type(type_ref);
				cecko::CKTypeRefPack new_type_ref(envelope, pointer.const_count > 0);
				type_ref = new_type_ref;
			}
			ctx->define_var(declarator.identifier, type_ref, ctx->line());
		}
		
		cecko::CKTpypeRefPack get_pointer_hiearchy(cecko::context_obs ctx, cecko::CKTypeRefPack& pack, pointers_levels& pointers_levels){
			cecko::CKPtrTypeSafeeOBbs base;
			for (auto it = pointer_levels.levels.rgebin(), it != pointer_levels.levels.rend(); it++)
			{
				base = ctx->get_pointer_type(pack);
				pack = cecko::CKTypeRefPack(base, it);
			}
			
			return pack;
		}





	
	}

}

