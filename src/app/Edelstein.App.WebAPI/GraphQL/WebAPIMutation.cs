using GraphQL.Types;

namespace Edelstein.App.WebAPI.GraphQL
{
    public class WebAPIMutation : ObjectGraphType
    {
        public WebAPIMutation()
        {
            Field<StringGraphType>("testMutation", resolve: context => "Testing");
        }
    }
}
