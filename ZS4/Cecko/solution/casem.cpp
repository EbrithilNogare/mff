#include "casem.hpp"
#include <stdio.h>

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
			default: return ctx->get_void_type(); // not implemented
		}
	}

	cecko::CKTypeRefPack get_base_type(cecko::context* ctx, DeclarationSpecifiersDto specifiers){
		cecko::CKTypeObs type;
		size_t type_count(0);
		size_t const_count(0);
		size_t typedef_count(0);
		size_t type_ind(0);
		size_t const_ind(0);
		size_t typedef_ind(0);
		bool const_before_type(false);

		for (size_t i = 0; i < specifiers.size(); i++)
		{
			if(specifiers[i].is_const){
				const_count++;
				const_ind = i;

				if(type_count == 0){
					const_before_type = true;
				}
			}
			if(specifiers[i].is_typedef){
				typedef_count++;
				typedef_ind = i;
			}
			if(specifiers[i].is_type){
				type = specifiers[i].type;
				type_count++;
				type_ind = i;
			}
		}
		
		cecko::CKTypeRefPack decl_type(type, (const_count > 0));

		return decl_type;
	}

	// Scan the given declarators and complete the type.... 
	cecko::CKTypeRefPack envelope_type(cecko::context* ctx, cecko::CKTypeRefPack decl_type, DeclaratorDto declarator){
		std::reverse(declarator.modifiers.begin(), declarator.modifiers.end());
		for (auto&& modifier : declarator.modifiers) {
			if (modifier.type == ModifierType::pointer){				 
				std::reverse(modifier.pointers.begin(), modifier.pointers.end());
				for (auto&& pointer : modifier.pointers) { 
					auto ptr_type = ctx->get_pointer_type(decl_type);
					decl_type = cecko::CKTypeRefPack{ptr_type, pointer.is_const};
				} 
			} else if (modifier.type == ModifierType::array){
				auto arr_type = ctx->get_array_type(decl_type.type, modifier.array.size);
				decl_type = cecko::CKTypeRefPack{arr_type, false};
			} else if(modifier.type == ModifierType::function){
				std::vector<cecko::CKTypeObs> params;
				for (auto && param : modifier.function.parameters) {
					auto param_type = get_base_type(ctx, param.declarationSpecifiers);
					bool no_declarator = true;

					for(auto && param_declarator : param.declarators){
						param_type = envelope_type(ctx, param_type, param_declarator);
						params.push_back(param_type.type);
						no_declarator = false;
					}

					if(no_declarator == true){
						printf("😎-1\n");
						printf("😎-type? %s \n", param_type.type);
						printf("😎-2\n");
						printf("😎-int?: %s \n", param_type.type->is_int());
						printf("😎-3\n");
						if(param_type.type->is_int()){
							params.push_back(ctx->get_int_type());
						}
						if(param_type.type->is_char()){
							params.push_back(ctx->get_char_type());
						}
						if(param_type.type->is_bool()){
							params.push_back(ctx->get_bool_type());
						}
					}

				}

				auto func_type = ctx->get_function_type(decl_type.type, params);
				decl_type = cecko::CKTypeRefPack{func_type, false};
			}
		}
		return decl_type;
	}

	void declare(cecko::context* ctx, DeclarationSpecifiersDto specifiers, DeclaratorsDto declarators){
		cecko::CKTypeRefPack decl_type = get_base_type(ctx, specifiers);
		
		bool typedf = specifiers[0].is_typedef;

		for (auto &&declarator : declarators) {
			decl_type = envelope_type(ctx, decl_type, declarator);
		
			if(typedf){
				ctx->define_typedef(declarator.identifier, decl_type, declarator.line);
				continue;
			}
		
			if(	decl_type.type->is_int()||
				decl_type.type->is_bool() ||
				decl_type.type->is_char() ||
				decl_type.type->is_pointer() ||
				decl_type.type->is_array() ||
				decl_type.type->is_enum()){
				ctx->define_var(declarator.identifier, decl_type, declarator.line);
			}
			
			if(decl_type.type->is_function()){
				ctx->declare_function(declarator.identifier, decl_type.type, declarator.line);
			}
		
		}

	}

}

