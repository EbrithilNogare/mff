#include "casem.hpp"
#include <stdio.h>

namespace casem {
	cecko::CKTypeObs convert_etype(cecko::context* ctx, cecko::gt_etype etype){
		switch(etype){
			case cecko::gt_etype::BOOL: return ctx->get_bool_type();
			case cecko::gt_etype::CHAR: return ctx->get_char_type();
			case cecko::gt_etype::INT: return ctx->get_int_type();
			default: return ctx->get_void_type(); // not implemented
		}
	}

	cecko::CKTypeRefPack get_init_type(cecko::context* ctx, DeclarationSpecifiersDto specifiers){
		cecko::CKTypeObs type;
		bool contain_const = false;
		bool const_before_type = false;

		for (auto &&specifier: specifiers)
		{
			if(specifier.is_const)
				contain_const = true;
			if(specifier.is_type)
				type = specifier.type;
		}
		
		return cecko::CKTypeRefPack(type, contain_const);
	}

	// Scan the given declarators and complete the type.... 
	cecko::CKTypeRefPack apply_to_type(cecko::context* ctx, cecko::CKTypeRefPack declarator_type, DeclaratorDto declarator){
		std::reverse(declarator.modifiers.begin(), declarator.modifiers.end());
		
		for (auto&& modifier : declarator.modifiers) {
			if (modifier.type == ModifierType::pointer){				 
				std::reverse(modifier.pointers.begin(), modifier.pointers.end());
				for (auto&& pointer : modifier.pointers) { 
					auto ptr_type = ctx->get_pointer_type(declarator_type);
					declarator_type = cecko::CKTypeRefPack{ptr_type, pointer.is_const};
				} 
			}
			
			if (modifier.type == ModifierType::array){
				auto arr_type = ctx->get_array_type(declarator_type.type, modifier.array.size);
				declarator_type = cecko::CKTypeRefPack{arr_type, false};
			}
			
			if(modifier.type == ModifierType::function){
				std::vector<cecko::CKTypeObs> params;
				for (auto && param : modifier.function.parameters) {
					auto param_type = get_init_type(ctx, param.declarationSpecifiers);

					for(auto && param_declarator : param.declarators){
						param_type = apply_to_type(ctx, param_type, param_declarator);
						params.push_back(param_type.type);
					}

					if(param_type.type->is_int())
						params.push_back(ctx->get_int_type());
					
					if(param_type.type->is_char())
						params.push_back(ctx->get_char_type());
					
					if(param_type.type->is_bool())
						params.push_back(ctx->get_bool_type());

				}

				auto func_type = ctx->get_function_type(declarator_type.type, params);
				declarator_type = cecko::CKTypeRefPack{func_type, false};
			}
		}

		return declarator_type;
	}

	void declare(cecko::context* ctx, DeclarationSpecifiersDto specifiers, DeclaratorsDto declarators){
		cecko::CKTypeRefPack declarator_type = get_init_type(ctx, specifiers);
		
		bool typedf = specifiers[0].is_typedef;

		for (auto &&declarator : declarators) {
			declarator_type = apply_to_type(ctx, declarator_type, declarator);
		
			if(typedf){
				ctx->define_typedef(declarator.identifier, declarator_type, declarator.line);
				continue;
			}
		
			if(	declarator_type.type->is_int() ||
				declarator_type.type->is_bool() ||
				declarator_type.type->is_char() ||
				declarator_type.type->is_pointer() ||
				declarator_type.type->is_array() ||
				declarator_type.type->is_enum())
				ctx->define_var(declarator.identifier, declarator_type, declarator.line);
			
			if(declarator_type.type->is_function()) {
				ctx->declare_function(declarator.identifier, declarator_type.type, declarator.line);
			}
		
		}

	}

void declareFunctionDefinition(cecko::context* ctx, DeclarationSpecifiersDto specifiers, DeclaratorDto declarator){
	cecko::CKTypeRefPack declarator_type = get_init_type(ctx, specifiers);
	

	declarator_type = apply_to_type(ctx, declarator_type, declarator);

	if(declarator_type.type->is_function()) {
		auto func_object = ctx->declare_function(declarator.identifier, declarator_type.type, declarator.line);
		cecko::CKFunctionFormalPackArray params_prepared;
		
		printf("here");
		/*
		for(auto& param: declarator.modifiers){
			if(param.type != ModifierType::function) continue;
			params_prepared.emplace_back(param.function.parameters.declarators.name, false, param.loc);
		}
		*/
		printf("enter_function\n");
		ctx->enter_function(func_object, params_prepared, declarator.line);
	}


}
	

/*
	cecko::CKTypeRefPack get_pointer_hierarchy(cecko::context_obs ctx, cecko::CKTypeRefPack& pack, pointers_levels& pointers_levels){
		cecko::CKTypeRefSafePack base;
		for(auto it = pointers_levels.levels.rbegin(); it != pointers_levels.levels.rend(); it++){
			base = ctx->get_pointer_type(pack);
			pack = cecko:: CKTypeRefPack(base, *it);
		}

		return pack;
	}
	void enter_function_inner(cecko::context_obs ctx, DeclarationSpecifiersDto& specifiers, DeclaratorModifier& declarator, cecko::loc_t loc){
		CSProcessedSpecifier specifier = process_specifiers(ctx, specifiers, loc);
		cecko::CKTypeRefPack pack = specifier.pack;

		pack = get_pointer_hierarchy(ctx,pack, declarator.constness_pointer);

		cecko::CKTypeObs func_type = get_function_pointer_hierarchy_as_function(ctx, pack.type,declarator.parameter_levels, loc);
		auto func_object = ctx->declare_function(declarator.identifier,func_type,loc);

		cecko::CKFunctionFormalPackArray params_prepared;

		for(auto& param: declarator.parameters_levels[0]){
			if(!param.pack.type->is_void())
				params_prepared.emplace_back(param.name, false, param.loc);

		}

		ctx->enter_function(func_object, params_prepared, loc);
	}
*/
}