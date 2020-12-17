#ifndef casem_hpp_
#define casem_hpp_

#include "cktables.hpp"
#include "ckcontext.hpp"
#include "ckgrptokens.hpp"

namespace casem {
	struct CKParameter;
	using CKParameterList = std::vector<casem::CKParameter>;

	enum ModifierType {
		pointer,
		array,
		function,
	};

	struct CKDeclarationSpecifier {
		public:
		cecko::CKTypeObs type;
		bool is_typedef;
		bool is_const;
		bool is_type;

		CKDeclarationSpecifier() {}
		CKDeclarationSpecifier(bool is_typedef, bool is_const) {
			this->is_typedef = is_typedef;
			this->is_const = is_const;
			this->is_type = false;
			this->type = cecko::CKTypeObs();
		}
		CKDeclarationSpecifier(cecko::CKTypeObs type) {
			this->is_typedef = false;
			this->is_const = false;
			this->is_type = true;
			this->type = type;
		}
	};
	using CKDeclarationSpecifierList = std::vector<casem::CKDeclarationSpecifier>;

	struct CKPointer {
		public:
			bool is_const;

			CKPointer(bool is_const){ 
				this->is_const = is_const;
			}
	};
	using CKPointerList = std::vector<casem::CKPointer>; 
	
	struct CKArray {
		public:
			cecko::CKIRConstantIntObs size;

			CKArray() {}
			CKArray(cecko::CKIRConstantIntObs size){
				this->size = size;
			}
	};
	struct CKFunction{
		public:
			CKParameterList parameters;

			CKFunction(void) {}
			CKFunction(CKParameterList parameters){
				this->parameters = parameters;
			}
	};

	struct CKDeclaratorModifier{
		public:
			ModifierType type;
			CKPointerList pointers;
			CKArray array;
			CKFunction function;

			CKDeclaratorModifier(CKPointerList pointers){
				this->type = ModifierType::pointer;
				this->pointers = pointers;
			}
			CKDeclaratorModifier(CKArray array){
				this->type = ModifierType::array;
				this->array = array;
			}
			CKDeclaratorModifier(CKFunction function){
				this->type = ModifierType::function;
				this->function = function;
			}
	};	

	struct CKDeclarator{
		public:
			cecko::CIName identifier;
			cecko::loc_t line;
			std::vector<CKDeclaratorModifier> modifiers;
			
			CKDeclarator(void){
				this->identifier = cecko::CIName();
			}
			CKDeclarator(cecko::CIName identifier, cecko::loc_t line) {
				this->identifier = identifier;
				this->line = line;
			}

			void add_modifier(CKDeclaratorModifier modifier){
				this->modifiers.push_back(modifier);
			}
	};
	using CKDeclaratorList = std::vector<CKDeclarator>;

	struct CKParameter{
		public:
			CKDeclarationSpecifierList declarationSpecifiers;
			CKDeclaratorList declarators;
			
			CKParameter(void) {}
			CKParameter(CKDeclarationSpecifierList declarationSpecifiers){
				this->declarationSpecifiers = declarationSpecifiers;
			}
			CKParameter(CKDeclarationSpecifierList declarationSpecifiers, casem::CKDeclaratorList declarators){
				this->declarationSpecifiers = declarationSpecifiers;
				this->declarators = declarators;
			}
	};

	void declare(cecko::context* ctx, CKDeclarationSpecifierList specifiers, CKDeclaratorList declarators);
	void declareFunctionDefinition(cecko::context* ctx, CKDeclarationSpecifierList specifiers, CKDeclarator declarator);
	cecko::CKTypeObs convert_etype(cecko::context* ctx, cecko::gt_etype etype);


	enum CKExpressionType{
		// CKArrayType,
		CKBoolType,
		CKCharType,
		// CKEnumType,
		// CKFunctionType,
		CKIntType,
		CKPtrType,
		// CKStructType,
		CKVoidType,
		CKNIType, // not implemented
	};

	struct CKExpression{
		CKExpressionType type;
		//cecko::CIAbstractType value;

		CKExpression(){
			this->type = CKVoidType;
		}
		CKExpression(CKExpressionType type/*, cecko::CIAbstractType value*/){
			this->type = type;
			//this->value = value;
		}

	};








}

#endif
