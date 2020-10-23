#include "ckmain.hpp"

namespace cecko {

	template< typename AR>
	inline bool read_args(int argc, char** argv, AR&& arg_reader, std::string& input_fname, int& app_argc, char**& app_argv)
	{
		std::size_t aix = 1, apos = 0;

		auto getter = [argv, argc, &aix, &apos]() -> std::string {
			if (argv[aix][apos])
			{
				auto val = std::string(argv[aix] + apos);
				++aix;
				return val;
			}
			if (aix + 1 < argc)
			{
				auto val = std::string(argv[aix + 1]);
				aix += 2;
				return val;
			}
			return std::string();
		};

		while (aix < argc && (apos > 0 || argv[aix][0] == '-'))
		{
			// skip '-' if necessary
			if (apos == 0 && argv[aix][0] == '-')
				++apos;
			// read the option char
			char opt = argv[aix][apos];
			// move to the next char
			++apos;
			if (!argv[aix][apos])
			{
				++aix;
				apos = 0;
			}
			// call the reader, optionally moving through an argument
			auto rv = arg_reader(opt, getter);
			if (!rv)
			{
				std::cout << "Unrecognized switch \"" << opt << "\"" << std::endl;
				return false;
			}
		}

		if (aix >= argc)
		{
			std::cout << "cecko <input file>" << std::endl;
			return false;
		}

		input_fname = argv[aix];
		app_argc = argc - aix;
		app_argv = argv + aix;

		return true;
	}

	bool main_state_parser::dump_tables() const
	{
		out() << "========== tables ==========" << std::endl;
		the_tables.dump_tables(out());
		return true;
	}

	bool main_state_code::setup(int argc, char** argv)
	{
		auto arg_reader = [this](char opt, auto&& get_val) -> bool {
			switch (opt)
			{
			case 'o':
				if (!oname.empty())
					return false;
				oname = get_val();
				return true;
			case 'z':
				if (!!outp_owner_)
					return false;
				{
					auto outname = get_val();
					outp_owner_ = std::make_unique<std::ofstream>(outname);
					if (!outp_owner_->good())
					{
						outp_owner_.reset();
						return false;
					}
					outp_ = &*outp_owner_;
				}
				return true;
			default:
				return false;
			}
		};

		return read_args(argc, argv, arg_reader, input_fname, app_argc, app_argv);
	}

	bool main_state_code::dump_code() const
	{
		out() << "========== IR module ==========" << std::endl;
		{
			std::stringstream oss;
			the_tables.dump_ir_module(oss);
			for (;;)
			{
				std::string lbuf;
				auto rc = !!std::getline(oss, lbuf);
				if (!rc)
					break;
				out() << "::: " << lbuf << std::endl;
			}
		}

		if (!oname.empty())
		{
			auto oec = the_tables.write_bitcode_module(oname);
			if (!!oec)
			{
				std::cout << "Cannot open output file \"" << oname << "\": " << oec.message() << std::endl;
			}
			else
			{
				std::cout << "Module bitcode written into the file \"" << oname << "\"" << std::endl;
			}
		}
		return true;
	}

	bool main_state_code::run_code()
	{
		auto mainf = the_tables.globtable()->find_function("main");
		if (!mainf)
		{
			out() << "Cannot find main function." << std::endl;
			return false;
		}
		else
		{
			auto fnc = mainf->get_function_ir();
			irenv.run_main(fnc, app_argc, app_argv, out());
			return true;
		}
	}
}
