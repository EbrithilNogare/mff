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

	class DeclarationSpecifierDto {
		public:
		cecko::CKTypeObs type;
		bool is_typedef;
		bool is_const;
		bool is_type;

		DeclarationSpecifierDto(void):
			is_typedef(false), is_const(false), is_type(false), type() {}
		
		DeclarationSpecifierDto(bool typdef, bool cnst):
			is_typedef(typdef), is_const(cnst), is_type(false), type() {}
		
		DeclarationSpecifierDto(cecko::CKTypeObs type):
			is_typedef(false), is_const(false), is_type(true), type(type) {}
	};
	using DeclarationSpecifiersDto = std::vector<casem::DeclarationSpecifierDto>;

	class PointerDto {
		public:
			bool is_const;
			PointerDto(bool is_const): 
				is_const(is_const) {}
	};
	using PointersDto = std::vector<casem::PointerDto>; 
	
	class ArrayDto {
		public:
			cecko::CKIRConstantIntObs size;
			ArrayDto(cecko::CKIRConstantIntObs size) : size(size) {}
			ArrayDto() {}
	};
	class FunctionDto{
		public:
			ParametersDto parameters;
			FunctionDto() {}
			FunctionDto(ParametersDto parameters) : parameters(parameters) {}
	};

	class DeclaratorModifierDto{
		public:
			ModifierType type;
			PointersDto pointers;
			ArrayDto array;
			FunctionDto function;

			DeclaratorModifierDto(PointersDto ptrs) : pointers(ptrs), type(ModifierType::pointer) {}
			DeclaratorModifierDto(ArrayDto array) : array(array), type(ModifierType::array) {}
			DeclaratorModifierDto(FunctionDto func) : function(func), type(ModifierType::function) {}
	};	

	class DeclaratorDto{
		public:
			cecko::CIName identifier;
			cecko::loc_t line;
			std::vector<DeclaratorModifierDto> modifiers;
			
			bool is_anonym;
			bool is_var;
			bool is_func;

			DeclaratorDto(void): is_anonym(true), is_var(true), is_func(false), identifier() {}
			DeclaratorDto(cecko::CIName identifier, cecko::loc_t line): identifier(identifier), is_anonym(false), is_var(false), is_func(false), line(line) {}

			void add_modifier(DeclaratorModifierDto modifier, bool func){
				modifiers.push_back(modifier);
			}
	};
	using DeclaratorsDto = std::vector<DeclaratorDto>;

	class ParameterDto{
		public:
			DeclarationSpecifiersDto declarationSpecifiers;
			DeclaratorsDto declarators;
			ParameterDto() {}
			ParameterDto(DeclarationSpecifiersDto declarationSpecifiers): declarationSpecifiers(declarationSpecifiers) {}
			ParameterDto(DeclarationSpecifiersDto declarationSpecifiers, casem::DeclaratorsDto declarators): declarationSpecifiers(declarationSpecifiers), declarators(declarators) {}
	};

	void declare(cecko::context* ctx, DeclarationSpecifiersDto specifiers, DeclaratorsDto declarators);
	cecko::CKTypeObs parse_etype(cecko::context* ctx, cecko::gt_etype etype);

	class DeclarationBodyDto;

}

#endif
