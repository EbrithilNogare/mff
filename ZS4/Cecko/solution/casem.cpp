#include "casem.hpp"

namespace casem {

	void DefineVariables(cecko::context* ctx, DeclarationSpecifiers specifiers, Declataros declarators){

	}

	std::vector<DeclarationSpecifiersDto> create_DeclarationSpecifiersDto(){
		return std::vector<DeclarationSpecifiersDto>();
	}
	
	std::vector<DeclaratorsDto> create_DeclaratorsDto(){
		return std::vector<DeclaratorsDto>();
	}

	std::vector<DeclaratorsDto> create_PointersDto(){
		return std::vector<PointersDto>();
	}

	CreateDeclarators();

	cecko::CKTypeObs parse_etype(cecko::context* ctx, cecko::gt_etype etype){
		switch(etype){
			case cecko::gt_etype::BOOL: return ctx->get_bool_type();
			case cecko::gt_etype::CHAR: return ctx->get_char_type();
			case cecko::gt_etype::INT: return ctx->get_int_type();
			default: exit(1)
		}
	}

}

