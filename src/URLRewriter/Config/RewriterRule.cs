using System;

namespace MindHarbor.URLRewriter.Config
{
	/// <summary>
	/// Represents a rewriter rule.  A rewriter rule is composed of a pattern to search for and a string to replace
	/// the pattern with (if matched).
	/// </summary>
	[Serializable()]
	public class RewriterRule
	{

	    private Expression redirection;
	    private Expression rewriter;

	    public Expression Redirection {
	        get { return redirection; }
	        set { redirection = value; }
	    }

	    public Expression Rewriter {
	        get { return rewriter; }
	        set { rewriter = value; }
	    }
	}
}
