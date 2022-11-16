<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" version="4.0" encoding="UTF-16" indent="yes"/>

	<xsl:template match="/">
		<html>
			<head>
				<title>Page Title</title>
			</head>
			<body>
				<!-- nadpis -->
				<h2>Archlinux</h2>
				<!-- jmena vsech balicku -->
				<h3>balíčky:</h3>
				<table>
					<tbody>
						<tr>
							<xsl:template match="archLinux">
								<xsl:call-template name="balicky"></xsl:call-template>
							</xsl:template>
						</tr>
					</tbody>
				</table
			</body>
		</html>
	</xsl:template>
		
	
	
	<xsl:template name="balicky">
		<li>
		</li>
	</xsl:template>

	

	
	
	
	




</xsl:stylesheet>