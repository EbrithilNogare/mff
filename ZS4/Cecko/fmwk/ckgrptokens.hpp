/** @file

grptokens.hpp

Group tokens.

*/

#ifndef CECKO_GRPTOKENS_GUARD__
#define CECKO_GRPTOKENS_GUARD__

namespace cecko {

	/// INCDEC token
	enum class gt_incdec { INC, DEC };

	/// ADDOP token
	enum class gt_addop { ADD, SUB };

	/// DIVOP token
	enum class gt_divop { DIV, MOD };

	/// CMPO token
	enum class gt_cmpo { LT, LE, GT, GE };

	/// CMPE token
	enum class gt_cmpe { EQ, NE };

	/// CASS token
	enum class gt_cass { MULA, DIVA, MODA, ADDA, SUBA };
	
	/// ETYPE token
	enum class gt_etype { BOOL, CHAR, INT };

}

#endif