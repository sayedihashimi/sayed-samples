using System.CommandLine;
using System.CommandLine.Invocation;

namespace SayedHa.OpenAPIExplorer.ConsoleRunner {
    public class MyCommand : CommandBase {
        private IReporter _reporter;
        public MyCommand(IReporter reporter) {
            _reporter = reporter;
        }
        public override Command CreateCommand() =>
            new Command(name: "explore", description: "Explorer an OpenAPI spec") {
                CommandHandler.Create<string,bool>(async (openApiFilePath,verbose) => {
                    _reporter.EnableVerbose = verbose;
                    _reporter.WriteLine(VsAscii);
                    _reporter.WriteLine(string.Empty);
					_reporter.WriteLine($"openApiFilePath: {openApiFilePath}");
					_reporter.WriteLine($"verbose: {verbose}");
                    _reporter.WriteVerbose("verbose message here");
                    // added here to avoid async/await warning
                    await Task.Delay(1000);
                }),
                ArgumentOpenApiFilePath(),
                OptionVerbose(),
            };
        protected Option OptionPackages() =>
            new Option(new string[] { "--paramname" }, "TODO: update param description") {
                Argument = new Argument<string>(name: "paramname")
            };

        protected Argument ArgumentOpenApiFilePath() =>
            new Argument<string>(
                name: "openApiFilePath",
				description: "The path to the OpenAPI file to explore"
			);

        private string VsAscii = @"                                                                                
                                                                                
                                                    ******(*                    
                                                  ********/%%%#,                
                                                **********/%%%%%%%(.            
                                             .************/%%%%%%%%%%#/         
               ,(((((/                     .**************/%%%%%%%%%%%%%%#*     
            *(((((((((((*                ,****************/%%%%%%%%%%%%%%%%%#   
         /(((((((((((((((((.           ,******************/%%%%%%%%%%%%%%%%%#   
     ,(((((((((((((((((((((((*       *********************/%%%%%%%%%%%%%%%%%#   
   /****,*((((((((((((((((((((((   ***********************/%%%%%%%%%%%%%%%%%#   
   /********((((((((((((((((((((((/***********************/%%%%%%%%%%%%%%%%%#   
   /*********,(((((((((((((((((((((((********************,*##################   
   /***********,/((((((((((((((((((((((/***************   *##################   
   /************. /(((((((((((((((((((((((**********,     *##################   
   /************.   /((((((((((((((((((((((((*****        *##################   
   /************.     *((((((((((((((((((((((((/          *##################   
   /************.  .*****(((((((((((((((((((((((((*       *##################   
   /************. *********(((((((((((((((((((((((((/     *##################   
   /*************************/(((((((((((((((((((((((((*  *##################   
   /*********,*****************/(((((((((((((((((((((((((/*##################   
   /*****************************/(((((((((((((((((((((((//##################   
   /***************************,   *(((((((((((((((((((((//##################   
      ,**********************.       *(((((((((((((((((((//##################   
         .****************,            .(((((((((((((((((//##################   
             ***********                 .((((((((((((((///##################   
                ,****.                     ./(((((((((((///###############*     
                                              /(((((((((///###########/         
                                                *(((((((///#######(.            
                                                  *((((((//####,                
                                                    .((((/(/                    
                                                                                ";
    }
}
