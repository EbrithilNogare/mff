#ifndef casem_hpp_
#define casem_hpp_

#include "cktables.hpp"
#include "ckcontext.hpp"
#include "ckgrptokens.hpp"

namespace casem {

	class DeclarationSpecifiersDto {
		public:
		cecko::CKTypeObs type;
		bool is_typedef;
		bool is_const;
		bool is_type;

		DeclarationSpecifiersDto(void):
			is_typedef(false), is_const(false), is_type(false), type() {}
		
		DeclarationSpecifiersDto(bool typdef, bool cnst):
			is_typedef(typdef), is_const(cnst), is_type(false), type() {}
		
		DeclarationSpecifiersDto(cecko::CKTypeObs type):
			is_typedef(false), is_const(false), is_type(true), type(type) {}
	};

	class DeclaratorDto{
		public:
			cecko::CIName identifier;
			PointersDto poiters;
			bool is_empty;
			bool is_array;
			bool is_function;
			bool is_pointer;

			DeclaratorDto(void):
				is_empty(true), is_array(false), is_function(false), is_pointer(false), identifier(), pointers() {}

			DeclaratorDto(cecko::CIName identifier):
				identifier(identifier), is_empty(false) {}

			void add_pointer(PointersDto& ptr) { pointers = ptr; is_pointer = true; }

	}

	class PointerDto {
		public:
			bool is_const;

			PointerDto(bool is_const = false): 
				is_const(is_const) {}
	};

	

}

#endif
