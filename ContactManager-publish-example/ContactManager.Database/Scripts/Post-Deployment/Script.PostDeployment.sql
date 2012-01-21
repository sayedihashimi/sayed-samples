/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
if not exists (Select top 1 * from States)
BEGIN
	Insert into States(StateCode,Name) values('AL','Alabama')
	Insert into States(StateCode,Name) values('AK','Alaska')
	Insert into States(StateCode,Name) values('AZ','Arizona')
	Insert into States(StateCode,Name) values('AR','Arkansas')
	Insert into States(StateCode,Name) values('CA','California')
	Insert into States(StateCode,Name) values('CO','Colorado')
	Insert into States(StateCode,Name) values('CT','Connecticut')
	Insert into States(StateCode,Name) values('DE','Delaware')
	Insert into States(StateCode,Name) values('DC','District of Columbia')
	Insert into States(StateCode,Name) values('FL','Florida')
	Insert into States(StateCode,Name) values('GA','Georgia')
	Insert into States(StateCode,Name) values('HI','Hawaii')
	Insert into States(StateCode,Name) values('ID','Idaho')
	Insert into States(StateCode,Name) values('IL','Illinois')
	Insert into States(StateCode,Name) values('IN','Indiana')
	Insert into States(StateCode,Name) values('IA','Iowa')
	Insert into States(StateCode,Name) values('KS','Kansas')
	Insert into States(StateCode,Name) values('KY','Kentucky')
	Insert into States(StateCode,Name) values('LA','Louisiana')
	Insert into States(StateCode,Name) values('ME','Maine')
	Insert into States(StateCode,Name) values('MT','Montana')
	Insert into States(StateCode,Name) values('NE','Nebraska')
	Insert into States(StateCode,Name) values('NV','Nevada')
	Insert into States(StateCode,Name) values('NH','New Hampshire')
	Insert into States(StateCode,Name) values('NJ','New Jersey')
	Insert into States(StateCode,Name) values('NM','New Mexico')
	Insert into States(StateCode,Name) values('NY','New York')
	Insert into States(StateCode,Name) values('NC','North Carolina')
	Insert into States(StateCode,Name) values('ND','North Dakota')
	Insert into States(StateCode,Name) values('OH','Ohio')
	Insert into States(StateCode,Name) values('OK','Oklahoma')
	Insert into States(StateCode,Name) values('OR','Oregon')
	Insert into States(StateCode,Name) values('MD','Maryland')
	Insert into States(StateCode,Name) values('MA','Massachusetts')
	Insert into States(StateCode,Name) values('MI','Michigan')
	Insert into States(StateCode,Name) values('MN','Minnesota')
	Insert into States(StateCode,Name) values('MS','Mississippi')
	Insert into States(StateCode,Name) values('MO','Missouri')
	Insert into States(StateCode,Name) values('PA','Pennsylvania')
	Insert into States(StateCode,Name) values('RI','Rhode Island')
	Insert into States(StateCode,Name) values('SC','South Carolina')
	Insert into States(StateCode,Name) values('SD','South Dakota')
	Insert into States(StateCode,Name) values('TN','Tennessee')
	Insert into States(StateCode,Name) values('TX','Texas')
	Insert into States(StateCode,Name) values('UT','Utah')
	Insert into States(StateCode,Name) values('VT','Vermont')
	Insert into States(StateCode,Name) values('VA','Virginia')
	Insert into States(StateCode,Name) values('WA','Washington')
	Insert into States(StateCode,Name) values('WV','West Virginia')
	Insert into States(StateCode,Name) values('WI','Wisconsin')
	Insert into States(StateCode,Name) values('WY','Wyoming')
END
