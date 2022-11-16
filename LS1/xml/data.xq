xquery version "3.0";

declare namespace output = "http://www.w3.org/2010/xslt-xquery-serialization";
declare option output:method         "xhtml";
declare option output:doctype-public "-//W3C//DTD XHTML 1.0 Transitional//EN";
declare option output:doctype-system "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd";

<ul> //join
    {
        for $a in distinct-values(doc('data.xml')/archLinux/package/properities/size)
            for $b in doc('data++.xml')/units/unit
            where $a/@units = $b/shortcut
            return
                <li>
                    {$a/@units} = {$b/fullName}
                </li>
    }
</ul>