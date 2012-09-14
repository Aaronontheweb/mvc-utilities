#----------------------------------
# Project data for MVC.Utilities
#----------------------------------

Projects => {
	:language = "en-US",
	:project_url = "https://github.com/Aaronontheweb/mvc-utilities",
	:license_url = "https://github.com/Aaronontheweb/mvc-utilities/blob/master/license.txt",
	:mvcutilities => {
			:id => "mvc-utilities",
			:dir => "MVC.Utilities",
			:test_dir => "MVC.Utilities.Tests",
			:title => "MVC.Utilities",
			:description => "Utility classes designed for ASP.NET MVC; deals with encryption, routing, caching, authorization, and various other security issues. Designed by used with Dependency Injection.",
			:copyright => "Copyright © Aaron Stannard 2011-2012",
			:authors => "Aaron Stannard",
			:company => "StannardLabs",
			:guid => "8574f8cd-55cf-4d63-8eb4-f98396058297"
		},
	:bcrypt =>{
			:id => "mvc-utilities-bcrypt",
			:dir => "MVC.Utilities.BCrypt",
			:title => "MVC.Utilities.BCrypt",
			:description => "Blowfish Encyption (BCrypt) provider for MVC.Utilities",
			:copyright => "Copyright © Aaron Stannard 2011-2012",
			:authors => "Aaron Stannard",
			:company => "StannardLabs",
			:guid => "ee848347-460c-4775-9adb-525300205748",
			:dependencies => {
				:bcrypt => {
					:name => "BCrypt",
					:version => "1.0.0.0"
				}
			}
		},
	:azure => {
			:id => "mvc-utilities-azure",
			:dir => "MVC.Utilities.Azure",
			:title => "MVC.Utilities.Azure",
			:description => "Azure AppFabric Cache provider for MVC.Utilities",
			:copyright => "Copyright © Aaron Stannard 2011-2012",
			:authors => "Aaron Stannard",
			:company => "StannardLabs",
			:guid => "3d5b58db-60c0-4c05-a596-26a2df183b7b",
			:dependencies => {
				:bcrypt => {
					:name => "WindowsAzure.Caching",
					:version => "1.6.0.0"
				}
			}
		},
	:memcached => {
			:id => "mvc-utilities-memcached",
			:dir => "MVC.Utilities.Memcached",
			:test_dir => "MVC.Utilities.Memcached.Tests",
			:title => "Memcached provider for MVC.Utilities",
			:description => "",
			:copyright => "Copyright © Aaron Stannard 2011-2012",
			:authors => "Aaron Stannard",
			:company => "StannardLabs",
			:guid => "724b0477-f465-4e88-826f-888e83f59940"
			:dependencies => {
				:bcrypt => {
					:name => "WindowsAzure.Caching",
					:version => "1.6.0.0"
				}
			}
	}
}