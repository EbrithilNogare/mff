#pragma once

#include "cktables.hpp"
#include "ckcontext.hpp"
#include "ckgrptokens.hpp"

namespace casem {
	struct CKParameter;
	using CKParameterList = std::vector<CKParameter>;

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
	using CKDeclarationSpecifierList = std::vector<CKDeclarationSpecifier>;

	struct CKPointer {
		public:
			bool is_const;

			CKPointer(bool is_const){ 
				this->is_const = is_const;
			}
	};
	using CKPointerList = std::vector<CKPointer>; 
	
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
			CKParameter(CKDeclarationSpecifierList declarationSpecifiers, CKDeclaratorList declarators){
				this->declarationSpecifiers = declarationSpecifiers;
				this->declarators = declarators;
			}
	};

	void declare(cecko::context_obs ctx, CKDeclarationSpecifierList specifiers, CKDeclaratorList declarators);
	void declareFunctionDefinition(cecko::context_obs ctx, CKDeclarationSpecifierList specifiers, CKDeclarator declarator);
	cecko::CKTypeObs convert_etype(cecko::context_obs ctx, cecko::gt_etype etype);
	
	enum CKExpressionMode{
		rvalue, 
		lvalue,
	};

	enum CKExpressionOperator{
		addition,
		substraction,
		multiplication,
		division,
		modulo,
		addressing,
		dereferencing,
		negation,
		incrementing,
		decrementing,
		assign,
	};

	
	struct CKExpression{
		cecko::CKIRValueObs value;
		cecko::CKTypeObs type;
		CKExpressionMode mode;
		bool is_const;

		CKExpression(){}
		
		CKExpression(cecko::CKIRValueObs value, CKExpressionMode mode, cecko::CKTypeObs type, bool is_const){
			this->value = value;
			this->mode = mode;
			this->type = type;
			this->is_const = is_const;
		}

		CKExpression(cecko::context_obs ctx, int int_literal){
			value = ctx->get_int32_constant(int_literal);
			mode = CKExpressionMode::rvalue;
			type = ctx->get_int_type();
			is_const = true;
		}

		CKExpression(cecko::context_obs ctx, cecko::CIName string_literal){
			value = ctx->builder()->CreateGlobalStringPtr(string_literal);
			mode = CKExpressionMode::rvalue;
			is_const = true;
			cecko::CKTypeRefPack type_ref_pack = cecko::CKTypeRefPack(ctx->get_char_type(), is_const);
			type = ctx->get_pointer_type(type_ref_pack);
		}

		static CKExpression get_by_name(cecko::context_obs ctx, cecko::CIName name, cecko::loc_t line){
			cecko::CKNamedSafeObs expression = ctx->find(name);
			if(!expression){
				// error UNDEF_IDF at line: ${line}
				return CKExpression();
			}

			CKExpressionMode mode;
			if (expression->is_var()) {
				mode = CKExpressionMode::lvalue;
			} else {
				mode = CKExpressionMode::rvalue;
			}
			return CKExpression(expression->get_ir(), mode, expression->get_type(), expression->is_const());
		}

		/*
		static CKExpression value(void){
			CKExpression ex;
			return ex;
		}
		*/

	};
	using CKExpressionList = std::vector<CKExpression>;


	cecko::CKIRValueObs convert_to_rvalue(cecko::context_obs ctx, CKExpression operand, std::string note);
	CKExpression unary_operations(cecko::context_obs ctx, CKExpression operand, CKExpressionOperator op, cecko::loc_t loc, bool is_prefix);

	CKExpression binary_operations(cecko::context_obs ctx, CKExpression to, CKExpression from, CKExpressionOperator op, cecko::loc_t loc);
	CKExpression assigment(cecko::context_obs ctx, CKExpression to, CKExpression from, CKExpressionOperator op, cecko::loc_t loc);

	CKExpressionOperator get_cass_type(cecko::gt_cass cass);

	CKExpressionOperator get_incdec_type(cecko::gt_incdec incdec);
	CKExpressionOperator get_addop_type(cecko::gt_addop addop);
	CKExpressionOperator get_divop_type(cecko::gt_divop divop);
	
	CKExpression call_function(cecko::context_obs ctx, CKExpression f, CKExpressionList args, cecko::loc_t loc);

	void return_function(cecko::context_obs ctx, CKExpression expression);
	

	CKExpression get_sizeof(cecko::context_obs ctx, casem::CKDeclarationSpecifierList t);



}