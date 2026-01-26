#r "nuget: Humanizer, 2.14.1"

using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Humanizer;


// Helper to get the location of this script file
string GetScriptFolder([CallerFilePath] string path = "") => Path.GetDirectoryName(path);
string scriptDirectory = GetScriptFolder();

// Get the Solution Root 
string solutionRoot = Directory.GetParent(scriptDirectory).FullName;

string GetArgument(string name) => Environment.GetCommandLineArgs().FirstOrDefault(a => a.StartsWith($"-{name}="))?.Split('=')[1];
string featureName = GetArgument("FeatureName");
string endpointsArg = GetArgument("CreateEndpoint");

if (string.IsNullOrWhiteSpace(featureName))
{
    Console.WriteLine("Error: FeatureName is required.");
    return;
}

string basePath = Path.Combine(solutionRoot, "Features", featureName);


Console.WriteLine($"--------------------------Œ·Ì »«·ﬂ „‰ «·»«À Ì «⁄ﬂ «ﬂ » «·»«À ’Õ «»Ê”  «Ìœﬂ ------------------------");
Console.WriteLine($"GENERATING FEATURE: {featureName}");
Console.WriteLine($"TARGET PATH: {basePath}");
Console.WriteLine($"--------------------------------------------------");



string[] endpoints = (string.IsNullOrWhiteSpace(endpointsArg) || endpointsArg.Equals("All", StringComparison.OrdinalIgnoreCase))
    ? new[] { "Create", "GetAll", "GetById", "Update", "Delete" }
    : endpointsArg.Split(',', StringSplitOptions.RemoveEmptyEntries);

void WriteFile(string path, string content)
{
    Directory.CreateDirectory(Path.GetDirectoryName(path)!);
    File.WriteAllText(path, content.Trim());
    Console.WriteLine($"Created: {Path.GetFileName(path)}");
}

foreach (var action in endpoints)
{
    GenerateFeature(action.Trim());
}

void GenerateFeature(string action)
{
    var plural = featureName.Pluralize();
    bool isQueryAction = action.Equals("GetAll", StringComparison.OrdinalIgnoreCase) || action.Equals("GetById", StringComparison.OrdinalIgnoreCase);
    string actionLabel = action.Equals("GetAll", StringComparison.OrdinalIgnoreCase) ? $"GetAll{plural}" : $"{action}{featureName}";
    string folder = Path.Combine(basePath, actionLabel);
    string ns = $"Rahhal.Features.{featureName}.{actionLabel}";

    string viewModelType = action.Equals("GetAll") ? $"PagingViewModel<{actionLabel}ResponseViewModel>" : $"{actionLabel}ResponseViewModel";

    // 1. ViewModels
    WriteFile(Path.Combine(folder, "ViewModels", $"{actionLabel}RequestViewModel.cs"),
    $@"using FluentValidation;
    namespace {ns}.ViewModels {{
    public class {actionLabel}RequestViewModel {{ 
        {(action.Equals("GetAll") ? "public int PageNumber { get; set; } = 1; public int PageSize { get; set; } = 50; public bool SortByLastAdded { get; set; } = false;" : "")}
    }}
    public class {actionLabel}RequestViewModelValidator : AbstractValidator<{actionLabel}RequestViewModel> {{
        public {actionLabel}RequestViewModelValidator() {{ }}
    }}
   }}");

    WriteFile(Path.Combine(folder, "ViewModels", $"{actionLabel}ResponseViewModel.cs"),
    $@"namespace {ns}.ViewModels {{
    public class {actionLabel}ResponseViewModel {{ 
        public string ID {{ get; set; }}
    }}
}}");

   
    if (isQueryAction)
    {
        // QUERY PATTERN
        string queryPath = Path.Combine(folder, "Queries");
        WriteFile(Path.Combine(queryPath, $"{actionLabel}Query.cs"),
       $@"using MediatR;
       using Microsoft.EntityFrameworkCore;
       using Rahhal.Common.BaseEndpoint;
       using Rahhal.Common.Views;
       using Rahhal.Common.BaseHandler;
       using Rahhal.Common.Helper;
       using {ns}.ViewModels;
 
 namespace {ns}.Queries {{
        //  ‘ÊÌÂ  «‰Ì ÊÂ«ÃÌ «ﬂ »·ﬂ «·ﬂÊœ » «ÌœÌ 
     public record {actionLabel}Query({actionLabel}RequestViewModel ViewModel) : IRequest<RequestResult<{viewModelType}>>;
 
    public class {actionLabel}QueryHandler : BaseRequestHandler<{actionLabel}Query, RequestResult<{viewModelType}>> {{
        public {actionLabel}QueryHandler(BaseRequestHandlerParameter parameters) : base(parameters) {{ }}

        public override async Task<RequestResult<{viewModelType}>> Handle({actionLabel}Query request, CancellationToken cancellationToken) {{
            {(action.Equals("GetAll") ? @"
            var baseQuery = Enumerable.Empty<object>().AsQueryable(); 
            baseQuery = ApplyFilters(baseQuery, request.ViewModel);
            
            var projection = baseQuery.Select(x => new " + actionLabel + @"ResponseViewModel { });
            var result = await PagingHelper.CreateAsync(projection, request.ViewModel.PageNumber, request.ViewModel.PageSize);
            return RequestResult<" + viewModelType + @">.Success(result);" : @"
            return RequestResult<" + viewModelType + @">.Success(new " + actionLabel + @"ResponseViewModel());")}
        }}

        {(action.Equals("GetAll") ? $@"private static IQueryable<object> ApplyFilters(IQueryable<object> query, {actionLabel}RequestViewModel vm) {{
            return query;
        }}" : "")}
    }}
}}");
    }
    else
    {
        
        string logicPath = Path.Combine(folder, "Command");
        string dtoType = $"{actionLabel}ResponseDTO";

        
        WriteFile(Path.Combine(logicPath, $"{actionLabel}Orchestrator.cs"),
        $@"using MediatR;
        using Mapster;
        using Rahhal.Common.BaseEndpoint;
        using Rahhal.Common.BaseHandler;
        using Rahhal.Common.Views;
        using {ns}.ViewModels;
        using {ns}.DTOs;

namespace {ns}.Command {{
    public record {actionLabel}Orchestrator({actionLabel}RequestViewModel ViewModel) : IRequest<RequestResult<{actionLabel}ResponseViewModel>>;

    public class {actionLabel}OrchestratorHandler : BaseRequestHandler<{actionLabel}Orchestrator, RequestResult<{actionLabel}ResponseViewModel>> {{
        public {actionLabel}OrchestratorHandler(BaseRequestHandlerParameter parameters) : base(parameters) {{ }}

        public override async Task<RequestResult<{actionLabel}ResponseViewModel>> Handle({actionLabel}Orchestrator request, CancellationToken cancellationToken) {{
            var dto = request.ViewModel.Adapt<DTOs.{actionLabel}RequestDTO>();
            var result = await _mediator.Send(new {actionLabel}Command(dto));

            if (!result.IsSuccess)
                return RequestResult<{actionLabel}ResponseViewModel>.Failure(result.ErrorCode, result.Message);

            return RequestResult<{actionLabel}ResponseViewModel>.Success(result.Data.Adapt<{actionLabel}ResponseViewModel>());
        }}
    }}
}}");

      
        WriteFile(Path.Combine(logicPath, $"{actionLabel}Command.cs"),
        $@"using MediatR;
        using Rahhal.Common.BaseEndpoint;
        using Rahhal.Common.Views;
        using Rahhal.Common.BaseHandler;
        using {ns}.DTOs;

namespace {ns}.Command {{
    public record {actionLabel}Command({actionLabel}RequestDTO DTO) : IRequest<RequestResult<{actionLabel}ResponseDTO>>;

    public class {actionLabel}CommandHandler : BaseRequestHandler<{actionLabel}Command, RequestResult<{actionLabel}ResponseDTO>> {{
        public {actionLabel}CommandHandler(BaseRequestHandlerParameter parameters) : base(parameters) {{ }}

        public override async Task<RequestResult<{actionLabel}ResponseDTO>> Handle({actionLabel}Command request, CancellationToken cancellationToken) {{
            return RequestResult<{actionLabel}ResponseDTO>.Success(new {actionLabel}ResponseDTO());
        }}
    }}
 }}");

        // DTOs
        WriteFile(Path.Combine(folder, "DTOs", $"{actionLabel}RequestDTO.cs"), $"namespace {ns}.DTOs " +
            $"{{ " +
            $"public class {actionLabel}RequestDTO " +
            $"{{" +

            $" }} " +
            $"}}");
        WriteFile(Path.Combine(folder, "DTOs", $"{actionLabel}ResponseDTO.cs"), $"namespace {ns}.DTOs {{ public class {actionLabel}ResponseDTO {{ }} }}");
    }

    // 3. Endpoint
    string verb = action switch { "Create" => "HttpPost", "Update" => "HttpPatch", "Delete" => "HttpDelete", _ => "HttpGet" };
    string mediatorCall = isQueryAction ? $"new Queries.{actionLabel}Query(model)" : $"new Logic.{actionLabel}Orchestrator(model)";

    WriteFile(Path.Combine(folder, $"{actionLabel}Endpoint.cs"),
    $@"using Microsoft.AspNetCore.Mvc;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.Views;
using {ns}.ViewModels;

namespace {ns} {{
    public class {actionLabel}Endpoint : BaseEndpoint<{actionLabel}RequestViewModel, {viewModelType}> {{
        public {actionLabel}Endpoint(BaseEndpointParameters<{actionLabel}RequestViewModel> parameters) : base(parameters) {{ }}

        [{verb}(""{featureName}/{action}"")]
        public async Task<EndpointResponse<{viewModelType}>> HandleAsync([From{(verb == "HttpGet" ? "Query" : "Body")}] {actionLabel}RequestViewModel model) {{
            var validation = ValidateRequest(model);
            if (!validation.IsSuccess) return validation;

            var result = await _mediator.Send({mediatorCall});
            
            if(!result.IsSuccess) 
                return EndpointResponse<{viewModelType}>.Failure(result.ErrorCode, result.Message);

            return EndpointResponse<{viewModelType}>.Success(result.Data);
        }}
    }}
}}");
}