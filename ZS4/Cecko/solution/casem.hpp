#ifndef casem_hpp_
#define casem_hpp_

#include "cktables.hpp"
#include "ckcontext.hpp"
#include "ckgrptokens.hpp"

namespace casem {
	class ParameterDto;
	using ParametersDto = std::vector<casem::ParameterDto>;

	enum ModifierType {
		pointer,
		array,
		function,
	};

	struct DeclarationSpecifierDto {
		public:
		cecko::CKTypeObs type;
		bool is_typedef;
		bool is_const;
		bool is_type;

		DeclarationSpecifierDto() {}
		DeclarationSpecifierDto(bool is_typedef, bool is_const) {
			this->is_typedef = is_typedef;
			this->is_const = is_const;
			this->is_type = false;
			this->type = cecko::CKTypeObs();
		}
		DeclarationSpecifierDto(cecko::CKTypeObs type) {
			this->is_typedef = false;
			this->is_const = false;
			this->is_type = true;
			this->type = type;
		}
	};
	using DeclarationSpecifiersDto = std::vector<casem::DeclarationSpecifierDto>;

	struct PointerDto {
		public:
			bool is_const;

			PointerDto(bool is_const){ 
				this->is_const = is_const;
			}
	};
	using PointersDto = std::vector<casem::PointerDto>; 
	
	struct ArrayDto {
		public:
			cecko::CKIRConstantIntObs size;

			ArrayDto() {}
			ArrayDto(cecko::CKIRConstantIntObs size){
				this->size = size;
			}
	};
	struct FunctionDto{
		public:
			ParametersDto parameters;

			FunctionDto(void) {}
			FunctionDto(ParametersDto parameters){
				this->parameters = parameters;
			}
	};

	struct DeclaratorModifierDto{
		public:
			ModifierType type;
			PointersDto pointers;
			ArrayDto array;
			FunctionDto function;

			DeclaratorModifierDto(PointersDto pointers){
				this->type = ModifierType::pointer;
				this->pointers = pointers;
			}
			DeclaratorModifierDto(ArrayDto array){
				this->type = ModifierType::array;
				this->array = array;
			}
			DeclaratorModifierDto(FunctionDto function){
				this->type = ModifierType::function;
				this->function = function;
			}
	};	

	struct DeclaratorDto{
		public:
			cecko::CIName identifier;
			cecko::loc_t line;
			std::vector<DeclaratorModifierDto> modifiers;
			
			DeclaratorDto(void){
				this->identifier = cecko::CIName();
			}
			DeclaratorDto(cecko::CIName identifier, cecko::loc_t line) {
				this->identifier = identifier;
				this->line = line;
			}

			void add_modifier(DeclaratorModifierDto modifier){
				this->modifiers.push_back(modifier);
			}
	};
	using DeclaratorsDto = std::vector<DeclaratorDto>;

	struct ParameterDto{
		public:
			DeclarationSpecifiersDto declarationSpecifiers;
			DeclaratorsDto declarators;
			
			ParameterDto(void) {}
			ParameterDto(DeclarationSpecifiersDto declarationSpecifiers){
				this->declarationSpecifiers = declarationSpecifiers;
			}
			ParameterDto(DeclarationSpecifiersDto declarationSpecifiers, casem::DeclaratorsDto declarators){
				this->declarationSpecifiers = declarationSpecifiers;
				this->declarators = declarators;
			}
	};

	void declare(cecko::context* ctx, DeclarationSpecifiersDto specifiers, DeclaratorsDto declarators);
	cecko::CKTypeObs convert_etype(cecko::context* ctx, cecko::gt_etype etype);
}

#endif
