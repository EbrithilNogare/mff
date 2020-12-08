#ifndef casem_hpp_
#define casem_hpp_

#include "cktables.hpp"
#include "ckcontext.hpp"
#include "ckgrptokens.hpp"

namespace casem {
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
			int const_count;
			PointerDto(int const_count = 0): 
				const_count(const_count) {}
	};
	using PointersDto = std::vector<casem::PointerDto>; 
	
	class DeclaratorDto{
		public:
			cecko::CIName identifier;
			PointersDto pointers;
			bool is_empty;
			bool is_array;
			bool is_function;
			bool is_pointer;

			DeclaratorDto(void):
				is_empty(true), is_array(false), is_function(false), is_pointer(false), identifier(), pointers() {}

			DeclaratorDto(cecko::CIName identifier):
				identifier(identifier), is_empty(false) {}

			void add_pointer(PointersDto& ptr) { pointers = ptr; is_pointer = true; }

	};
	using DeclaratorsDto = std::vector<casem::DeclaratorDto>;

	void DefineVariables(cecko::context* ctx, DeclarationSpecifiersDto specifiers, DeclaratorsDto declarators);
	cecko::CKTypeObs parse_etype(cecko::context* ctx, cecko::gt_etype etype);

	class DeclarationBodyDto;

}

#endif
