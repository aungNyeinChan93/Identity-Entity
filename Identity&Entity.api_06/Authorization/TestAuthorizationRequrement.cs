using Microsoft.AspNetCore.Authorization;

namespace Identity_Entity.api_06.Authorization
{
    public class TestAuthorizationRequrement : IAuthorizationRequirement
    {

        public int Age { get; set; }

        public TestAuthorizationRequrement(int age)
        {
            Age = age;
        }
    }


    public class TestAuthorizationRequrementHandler : AuthorizationHandler<TestAuthorizationRequrement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TestAuthorizationRequrement requirement)
        {
            var hasAgeClaim = context.User.HasClaim(x => x.Type == "age");
            if (!hasAgeClaim)
            {   
                return Task.CompletedTask;
            }

            if (int.TryParse(context.User.FindFirst(x=>x.Type == "age")!.Value,out var age))
            {
                if (age < requirement.Age)
                {
                    return Task.CompletedTask;
                }

                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
