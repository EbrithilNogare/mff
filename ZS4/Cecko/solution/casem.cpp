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

	cecko::CKTypeRefPack get_init_type(cecko::context* ctx, CKDeclarationSpecifierList specifiers){
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
	cecko::CKTypeRefPack apply_to_type(cecko::context* ctx, cecko::CKTypeRefPack declarator_type, CKDeclarator declarator){
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
					bool skip_defaults = false;

					for(auto && param_declarator : param.declarators) {
						param_type = apply_to_type(ctx, param_type, param_declarator);
						params.push_back(param_type.type);
						skip_defaults = true;
					}

					if(!skip_defaults && param_type.type->is_int())
						params.push_back(ctx->get_int_type());
					
					if(!skip_defaults && param_type.type->is_char())
						params.push_back(ctx->get_char_type());
					
					if(!skip_defaults && param_type.type->is_bool())
						params.push_back(ctx->get_bool_type());
				}

				auto func_type = ctx->get_function_type(declarator_type.type, params);
				declarator_type = cecko::CKTypeRefPack{func_type, false};
			}
		}

		return declarator_type;
	}

	void declare(cecko::context* ctx, CKDeclarationSpecifierList specifiers, CKDeclaratorList declarators){
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

	void declareFunctionDefinition(cecko::context* ctx, CKDeclarationSpecifierList specifiers, CKDeclarator declarator){
		cecko::CKTypeRefPack declarator_type = get_init_type(ctx, specifiers);
		
		declarator_type = apply_to_type(ctx, declarator_type, declarator);

		if(!declarator_type.type->is_function()) {
			printf("!!! !declarator_type.type->is_function()\n");
			return;
		}
		

		auto func_object = ctx->declare_function(declarator.identifier, declarator_type.type, declarator.line);
		cecko::CKFunctionFormalPackArray params_prepared;
		

		for(auto& param: declarator.modifiers[0].function.parameters){
			if(param.declarationSpecifiers[0].type->is_void()) continue;
		
			params_prepared.emplace_back(param.declarators[0].identifier, param.declarationSpecifiers[0].is_const, declarator.line);
		}
		
		ctx->enter_function(func_object, params_prepared, declarator.line);

	}
	
}