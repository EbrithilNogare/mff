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
			auto decl_type = apply_to_type(ctx, declarator_type, declarator);
		
			if(typedf){
				ctx->define_typedef(declarator.identifier, decl_type, declarator.line);
				continue;
			}
		
			if(	decl_type.type->is_int() ||
				decl_type.type->is_bool() ||
				decl_type.type->is_char() ||
				decl_type.type->is_pointer() ||
				decl_type.type->is_array() ||
				decl_type.type->is_enum())
				ctx->define_var(declarator.identifier, decl_type, declarator.line);
			
			if(decl_type.type->is_function()) {
				ctx->declare_function(declarator.identifier, decl_type.type, declarator.line);
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
			if(param.declarationSpecifiers[0].type && param.declarationSpecifiers[0].type->is_void()) continue;
		
			params_prepared.emplace_back(param.declarators[0].identifier, false, declarator.line);
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
					result = operandRvalue;
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
					result = operandRvalue;
				break;
				case CKExpressionOperator::dereferencing:
					refpack = type->get_pointer_points_to();
					return CKExpression(operandRvalue, CKExpressionMode::lvalue, refpack.type , false);
				break;
				case CKExpressionOperator::addressing:
					refpack = cecko::CKTypeRefPack(type, operand.is_const);
					return CKExpression(operand.value, CKExpressionMode::rvalue, ctx->get_pointer_type(refpack), operand.is_const);
				break;
			default:
				printf("😥unsupported unary_operations\n");
		}
		return CKExpression(result, CKExpressionMode::rvalue, type, false);
	}
	

	CKExpression binary_operations(cecko::context_obs ctx, CKExpression to, CKExpression from, CKExpressionOperator op, cecko::loc_t loc){
		cecko::CKTypeObs type1 = to.type;
		cecko::CKTypeObs type2 = from.type;
		cecko::CKIRValueObs operandRvalue1 = convert_to_rvalue(ctx, to, "operand");
		cecko::CKIRValueObs operandRvalue2 = convert_to_rvalue(ctx, from, "operand");

		cecko::CKIRValueObs result;
		cecko::CKIRValueObs changed;
		cecko::CKTypeRefPack refpack;

		if(type1->is_char() && !type2->is_char()) printf("😱is_char");
		if(type1->is_bool() && !type2->is_bool()) printf("😱is_bool");
		if(type1->is_int() && !type2->is_int()) printf("😱is_int");

		switch(op){
			case CKExpressionOperator::addition:
				if(type1->is_char() || type1->is_bool())
					changed = ctx->builder()->CreateAdd(operandRvalue1, operandRvalue2, "addition_i8");
				else if (type1->is_pointer())
					changed = ctx->builder()->CreateGEP(operandRvalue1, operandRvalue2, "pointer addition");
				else
					changed = ctx->builder()->CreateAdd(operandRvalue1, operandRvalue2, "addition_i32");
				break;

			case CKExpressionOperator::substraction:
				if(type1->is_char() || type1->is_bool())
					changed = ctx->builder()->CreateSub(operandRvalue1, operandRvalue2, "substraction");
				else if (type1->is_pointer()) {
					auto neg = ctx->builder()->CreateNeg(operandRvalue1, "result_unary_negation");
					changed = ctx->builder()->CreateGEP(neg, operandRvalue2, "pointer substraction");
				} else
					changed = ctx->builder()->CreateSub(operandRvalue1, operandRvalue2, "substraction");
				break;

			case CKExpressionOperator::multiplication :
				changed = ctx->builder()->CreateMul(operandRvalue1, operandRvalue2, "multiplication");
				break;

			case CKExpressionOperator::division:
				changed = ctx->builder()->CreateSDiv(operandRvalue1, operandRvalue2, "division");
				break;

			case CKExpressionOperator::modulo:
				changed = ctx->builder()->CreateSRem(operandRvalue1, operandRvalue2, "modulo");
				break;

			default:
				printf("binary 😥");
		}
		return CKExpression(changed, CKExpressionMode::rvalue, to.type, to.is_const);
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

	CKExpressionOperator get_divop_type(cecko::gt_divop divop){
		switch(divop)
		{
			case cecko::gt_divop::DIV: return CKExpressionOperator::division;
			case cecko::gt_divop::MOD: return CKExpressionOperator::modulo;
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


	CKExpression call_function(cecko::context_obs ctx, CKExpression f, CKExpressionList args, cecko::loc_t loc){
		std::vector<cecko::CKIRValueObs> parameters;

		for (size_t i = 0; i < args.size(); i++)
		{
			auto arg = args[i];
			cecko::CKIRValueObs result = convert_to_rvalue(ctx, arg, "arg");


			auto typeRequired = f.type->get_function_arg_type(i);

			if(arg.type == ctx->get_int_type() && typeRequired == ctx->get_char_type())
				result = ctx->builder()->CreateTrunc(result, ctx->get_char_type()->get_ir(), "trunc");
			else if(arg.type == ctx->get_char_type() && typeRequired == ctx->get_int_type())
				result = ctx->builder()->CreateZExt(result, typeRequired->get_ir(), "zext");
			else if(arg.type == ctx->get_bool_type() && typeRequired == ctx->get_int_type())
				result = ctx->builder()->CreateZExt(result, typeRequired->get_ir(), "zext");
			else if(arg.type->is_array() && typeRequired->is_pointer())
				result = ctx->builder()->CreateConstInBoundsGEP2_32(arg.type->get_ir(), arg.value, 0, 0);
			

			parameters.push_back(result);
		}

		cecko::CKIRValueObs result = ctx->builder()->CreateCall(f.value, parameters);

		if(f.type->get_function_return_type() == ctx->get_void_type())
			return CKExpression();
		else
			return CKExpression(result, CKExpressionMode::rvalue, f.type->get_function_return_type(), true);
	}


	void return_function(cecko::context_obs ctx, CKExpression expression){
		auto return_type = ctx->current_function_return_type();
		auto return_value_rvalue = convert_to_rvalue(ctx, expression, "return_value");
		if(return_type->is_int() && (expression.type->is_char() || expression.type->is_bool()))
			return_value_rvalue = ctx->builder()->CreateZExt(return_value_rvalue, ctx->get_int_type()->get_ir(), "CreateZExt");
		else if(return_type->is_char() && expression.type->is_int())
			return_value_rvalue = ctx->builder()->CreateTrunc(return_value_rvalue, ctx->get_char_type()->get_ir(), "CreateTrunc");

		ctx->builder()->CreateRet(return_value_rvalue);
		ctx->builder()->ClearInsertionPoint();
	}


	CKExpression get_sizeof(cecko::context_obs ctx, casem::CKDeclarationSpecifierList t){
		if(t[0].type->is_bool()) return CKExpression(ctx, 1);
		if(t[0].type->is_char()) return CKExpression(ctx, 1);
		if(t[0].type->is_int()) return CKExpression(ctx, 4);
		return CKExpression(ctx, 1);
	}



}