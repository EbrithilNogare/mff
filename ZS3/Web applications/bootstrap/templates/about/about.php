<h2>About Front Controllers</h2>
<p>
	The front controller software design pattern is listed in several pattern catalogs and related to the design of web applications. It is "a controller that handles all requests for a website", which is a useful structure for web application developers to achieve the flexibility and reuse without code redundancy. 
</p>

<p>
	Front controllers are often used in web applications to implement workflows. While not strictly required, it is much easier to control navigation across a set of related pages (for instance, multiple pages used in an online purchase) from a front controller than it is to make the individual pages responsible for navigation. 
</p>

<p>
	The front controller may be implemented as a Java object, or as a script in a script language like PHP, Perl6, Python or Ruby that is called on every request of a web session. This script, for example an index.php, would handle all tasks that are common to the application or the framework, such as session handling, caching, and input filtering. Based on the specific request, it would then instantiate further objects and call methods to handle the particular task(s) required. 
</p>

<p>
	The alternative to a front controller would be individual scripts like login.php and order.php that would each then satisfy the type of request. Each script would have to duplicate code or objects that are common to all tasks. However, each script might also have more flexibility to implement the particular task required. 
</p>
