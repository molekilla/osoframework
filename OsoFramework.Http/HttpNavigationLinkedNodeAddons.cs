// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Text;

namespace OsoFramework.Http
{
	public static class HttpNavigationLinkedNodeAddons
	{
        public static HttpNavigation Parent(this LinkedListNode<HttpNavigationStep> step)
        {
            return step.List as HttpNavigation;
        }

        public static HttpNavigationStep Read(this LinkedListNode<HttpNavigationStep> step)
        {
            step.Value.ResponseData = step.Parent().Read(step.Value);
            return step.Value;
        }
        public static HttpNavigationStep Read(this LinkedListNode<HttpNavigationStep> step, params string[] parameters)
        {
            step.Value.ResponseData = step.Parent().Read(step.Value, parameters);
            return step.Value;
        }
	}
}
