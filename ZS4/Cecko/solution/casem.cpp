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
	

	cecko::CKIRValueObs convert_to_rvalue(cecko::context_obs ctx, CKExpression operand, std::string note){
		cecko::CKIRValueObs operandRvalue;
		if(operand.mode == CKExpressionMode::lvalue){
			return ctx->builder()->CreateLoad(operand.type->get_ir(), operand.value, "load");
		}
		else{
			return operand.value;
		}

	}

	CKExpression unary_operations(cecko::context_obs ctx, CKExpression operand, CKExpressionOperator op, cecko::loc_t loc, bool is_prefix){  //maybe const

		cecko::CKTypeObs type = operand.type;
		cecko::CKIRValueObs operandRvalue = convert_to_rvalue(ctx, operand, "operand");

		cecko::CKIRValueObs result;
		cecko::CKIRValueObs changed;
		cecko::CKTypeRefPack refpack;

		switch(op){
			case CKExpressionOperator::addition:
				result = operandRvalue;
				break;
			case CKExpressionOperator::substraction:
				result = ctx->builder()->CreateNeg(operandRvalue, "result_unary_negation");
				break;
			case CKExpressionOperator::incrementing:
				if(type->is_char() ||type->is_bool())
					changed = ctx->builder()->CreateAdd(operandRvalue, ctx->get_int8_constant(1), "incrementing");
				else if (type->is_pointer())
					changed = ctx->builder()->CreateGEP(operandRvalue, ctx->get_int32_constant(1), "pointer incrementing");
				else
					changed = ctx->builder()->CreateAdd(operandRvalue, ctx->get_int32_constant(1), "increment");
		
				ctx->builder()->CreateStore(changed, operand.value);
				if(is_prefix)
					result = changed;
				else
				{}
				break;
			case CKExpressionOperator::decrementing:
				if(type->is_char() ||type->is_bool())
					changed = ctx->builder()->CreateAdd(operandRvalue, ctx->get_int8_constant(-1), "incrementing");
				else if (type->is_pointer())
					changed = ctx->builder()->CreateGEP(operandRvalue, ctx->get_int32_constant(-1), "pointer incrementing");
				else
					changed = ctx->builder()->CreateAdd(operandRvalue, ctx->get_int32_constant(-1), "increment");
		
				ctx->builder()->CreateStore(changed, operand.value);
				if(is_prefix)
					result = changed;
				else
				{}
				break;
		
		}
		return operand;
	}
	

	CKExpression binary_operations(cecko::context_obs ctx, CKExpression to, CKExpression from, CKExpressionOperator op, cecko::loc_t loc){
		cecko::CKTypeObs type = to.type;
		cecko::CKIRValueObs operandRvalue = convert_to_rvalue(ctx, to, "operand");

		cecko::CKIRValueObs result;
		cecko::CKIRValueObs changed;
		cecko::CKTypeRefPack refpack;

		switch(op){
			case CKExpressionOperator::addition:
				if(type->is_char() ||type->is_bool())
					changed = ctx->builder()->CreateAdd(operandRvalue, from.value, "incrementing");
				else if (type->is_pointer())
					changed = ctx->builder()->CreateGEP(operandRvalue, from.value, "pointer incrementing");
				else
					changed = ctx->builder()->CreateAdd(operandRvalue, from.value, "increment");
		
				ctx->builder()->CreateStore(changed, to.value);
				//	result = changed;
				break;
			case CKExpressionOperator::substraction:
				result = ctx->builder()->CreateNeg(operandRvalue, "result_unary_negation");
				if(type->is_char() ||type->is_bool())
					changed = ctx->builder()->CreateAdd(operandRvalue, ctx->get_int8_constant(-1), "incrementing");
				else if (type->is_pointer())
					changed = ctx->builder()->CreateGEP(operandRvalue, ctx->get_int32_constant(-1), "pointer incrementing");
				else
					changed = ctx->builder()->CreateAdd(operandRvalue, ctx->get_int32_constant(-1), "increment");
		
				ctx->builder()->CreateStore(changed, to.value);
				
				//	result = changed;
				break;
		
		}
		return to;
	}

	CKExpression assigment(cecko::context_obs ctx, CKExpression to, CKExpression from, CKExpressionOperator op, cecko::loc_t loc){

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
			result = ctx->builder()->CreateZExt(result, to.type->get_ir(), "zext");
		else if(type == ctx->get_bool_type() && to.type == ctx->get_int_type())
			result = ctx->builder()->CreateZExt(result, to.type->get_ir(), "zext");

		auto store_result = ctx->builder()->CreateStore(result, to.value);
		return CKExpression(result, CKExpressionMode::rvalue, to.type, to.is_const);
	}

	CKExpressionOperator get_cass_type(cecko::gt_cass cass){
		switch (cass)
		{
			case cecko::gt_cass::MULA: return CKExpressionOperator::multiplication;
			case cecko::gt_cass::DIVA: return CKExpressionOperator::division;
			case cecko::gt_cass::MODA: return CKExpressionOperator::modulo;
			case cecko::gt_cass::ADDA: return CKExpressionOperator::addition;
			case cecko::gt_cass::SUBA: return CKExpressionOperator::substraction;
			default: break;
		}
		return CKExpressionOperator::addition;
	}

	CKExpressionOperator get_incdec_type(cecko::gt_incdec incdec){
		
		switch(incdec)
		{
			case cecko::gt_incdec::INC: return CKExpressionOperator::incrementing;
			case cecko::gt_incdec::DEC: return CKExpressionOperator::decrementing;
			default: break;
		}
		return CKExpressionOperator::addition;
	}

	CKExpressionOperator get_addop_type(cecko::gt_addop addop){
		
		switch(addop)
		{
			case cecko::gt_addop::ADD: return CKExpressionOperator::addition;
			case cecko::gt_addop::SUB: return CKExpressionOperator::substraction;
			default: break;
		}
		return CKExpressionOperator::addition;
	}


	CKExpression call_function(cecko::context_obs ctx, CKExpression $1, $3, cecko::loc_t loc){

	}


	void return_function(cecko::context_obs ctx, CKExpression expression){
		auto return_type = ctx->current_function_return_type();
		auto return_value_rvalue = convert_to_rvalue(ctx, expression, "return_value");  // exprerssion.value();
		if(return_type->is_int() && (expression.type->is_char() || expression.type->is_bool()))
			return_value_rvalue = ctx->builder()->CreateZExt(return_value_rvalue, ctx->get_int_type()->get_ir(), "CreateZExt");
		else if(return_type->is_char() && expression.type->is_int())
			return_value_rvalue = ctx->builder()->CreateTrunc(return_value_rvalue, ctx->get_char_type()->get_ir(), "CreateTrunc");

		ctx->builder()->CreateRet(return_value_rvalue);
		ctx->builder()->ClearInsertionPoint();
	}


}