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

	void declare(cecko::context_obs ctx, CKDeclarationSpecifierList specifiers, CKDeclaratorList declarators);
	void declareFunctionDefinition(cecko::context_obs ctx, CKDeclarationSpecifierList specifiers, CKDeclarator declarator);
	cecko::CKTypeObs convert_etype(cecko::context_obs ctx, cecko::gt_etype etype);
	
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

	casem::unary_operation(cecko::context_obs ctx, CKExpression expression, CKExpressionOperator op, cecko::loc_t loc, bool is_const){




		cecko::CKIRValueObs result;
		cecko::CKIRValueObs changed
		cecko::CKITypeRefPack refpack;

		switch(op){
			case CKExpressionOperator::addition:
				result = operandRvalue;
				break;
			case CKExpressionOperator::substraction:
				result = ctx->builder()->Creating(operandRvalue, "result_unary_negation");
				break;
			case CKExpressionOperator::incrementing:
				if(type->is_char() ||type->is_bool())
					changed = ctx->builder()->CreateAdd(operrandRvalue, ctx->get_int8_constaint(1), "incrementing");
				else if (type->is_pointer())
					changed = ctx->builder()->CreateGEP(operrandRvalue, ctx->get_int32_constaint(1), "pointer incrementing");
				else
					changed = ctx->builder()CreateAdd(operrandRvalue, ctx->get_int32_constaint(1), "increment");
		
				ctx->builderCreateStore(changed, operand.value);
				if(is_prefix)
					result = changed;
				else
				{}
		
		}

	}

	casem::CKExpression assigment(cecko::context_obs ctx, CKExpression to, CKExpression from, CKExpressionOperator op, cecko::loc_t loc){

		cecko::CKIRValueObs result;
		cecko::CKTypeObs type;

		if(op != CKExpressionOperator::assign){
			CKExpression binary_result = binary_operations(ctx, to, from, op, loc);
			result = binary_result.value;
			type = binary_result.type;
		} else {
			result = convert_to_rvalue(ctx, from, "assignment");
			type = from.type;
		}

		if(type == ctx->get_int_type() && to.type == ctx->get_char_type())
			result = ctx->builder()->CreateTrunc(result, ctx->get_char_type()->get_ir(), "trunc");
		else if(type == ctx->get_char_type() && to.type == ctx->get_int_type())
			result = ctx->builder()->CreateZExt(result, to.type == ctx->get_ir(), "zext");
		else if(type == ctx->get_bool_type() && to.type == ctx->get_int_type())
			result = ctx->builder()->CreateZExt(result, to.type == ctx->get_ir(), "zext");

		auto store_result = ctx->builder()->CreateStore(result, to.value);
		return CKExpression(result, CKExpressionMode::rvalue, to.type, to.is_const)
	}

	CKExpressionOperator get_cass_type(cecko::gt_cass cass){
		switch (cass)
		{
			case cecko::gt_cass::MULA: return CKExpressionOperator::multiplication;
			case cecko::gt_cass::DIVA: return CKExpressionOperator::division;
			case cecko::gt_cass::MODA: return CKExpressionOperator::modulo;
			case cecko::gt_cass::ADDA: return CKExpressionOperator::addressing;
			case cecko::gt_cass::SUBA: return CKExpressionOperator::substraction;
			default: break;
		}
	}


	enum CKExpressionMode{
		rvalue, 
		lvalue,
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

	};


	void return_function(cecko::context_obs ctx, CTExpression expression){
		auto return_type = ctx->current_function_return_type();
		auto return_value_rvalue = convert_to_rvalue(ctx, expression.value(), "return_value");
		if(return_type->is_int() && (expression->type->is_char() || expression->type->is_bool()))
			return_value_rvalue = ctx->builder()->CreateZExt(return_value_rvalue, ctx-> get_int_type()->get_ir(), "CreateZExt");
		else if(return_type->is_char() && expression->type->is_int())
			return_value_rvalue = ctx->builder->CreateTrunc(return_value_rvalue, ctx->get_char_type()->get_ir(), "CreateTrunc");

		ctx->builder()->CreateRet(return_value_rvalue);
		ctx->builder()->ClearInsertionPoint();
	}





}

#endif
