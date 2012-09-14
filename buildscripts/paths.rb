#-----------------------
# Project Folders
#-----------------------
root_folder = File.expand_path("#{File.dirname(__FILE__)}/..")

Folders = {
	:root => root_folder,
	:tools => File.join(root_folder, "tools"),
	:out => File.join(root_folder, "build"),
	:nunit => File.join(root_folder, "tools", "nunit"),
	:nuget_bin => File.join(root_folder, ".nuget"),
	:nuget_build => File.join(root_folder, "build", "nuget"),

	:mvcutilities_tests => File.join(root_folder, Projects[:mvcutilities][:test_dir]),
	:memcached_tests => File.join(root_folder, Projects[:memcached][:test_dir]),

	:mvcutilities_nuspec => {
			:root => File.join(root_folder, "build", "nuget", Projects[:mvcutilities][:dir]),
			:lib => File.join(root_folder, "build", "nuget", Projects[:mvcutilities][:dir], "lib"),
			:net40 => File.join(root_folder, "build", "nuget", Projects[:mvcutilities][:dir], "lib", "net40"),
	},

	:azure_nuspec => {
			:root => File.join(root_folder, "build", "nuget", Projects[:azure][:dir]),
			:lib => File.join(root_folder, "build", "nuget", Projects[:azure][:dir], "lib"),
			:net40 => File.join(root_folder, "build", "nuget", Projects[:azure][:dir], "lib", "net40"),
	},

	:bcrypt_nuspec => {
			:root => File.join(root_folder, "build", "nuget", Projects[:bcrypt][:dir]),
			:lib => File.join(root_folder, "build", "nuget", Projects[:bcrypt][:dir], "lib"),
			:net40 => File.join(root_folder, "build", "nuget", Projects[:bcrypt][:dir], "lib", "net40"),
	},

	:memcached_nuspec => {
			:root => File.join(root_folder, "build", "nuget", Projects[:memcached][:dir]),
			:lib => File.join(root_folder, "build", "nuget", Projects[:memcached][:dir], "lib"),
			:net40 => File.join(root_folder, "build", "nuget", Projects[:memcached][:dir], "lib", "net40"),
	},

	:mvcutilities_bin => 'placeholder - specify build environment',
	:bcrypt_bin => 'placeholder - specify build environment',
	:azure_bin => 'placeholder - specify build environment',
	:memcached_bin => 'placeholder - specify build environment'
}

#-----------------------
# Project Files
#-----------------------

Files = {
	:solution => "MVC.Utilities.sln",
	:version => "VERSION",
	:assembly_info => "SharedAssemblyInfo.cs",
	:license => "license.txt",
	:readme => "README.textile",

	:mvcutilities => {
		:nuspec => "#{Projects[:mvcutilities][:id]}.nuspec",
		:test => "#{Projects[:mvcutilities][:test_dir]}.dll",
		:bin => "#{Projects[:mvcutilities][:dir]}.dll"
	},

	:bcrypt => {
		:nuspec => "#{Projects[:bcrypt][:id]}.nuspec",
		:bin => "#{Projects[:bcrypt][:dir]}.dll"
	},

	:azure => {
		:nuspec => "#{Projects[:azure][:id]}.nuspec",
		:bin => "#{Projects[:azure][:dir]}.dll"
	},

	:memcached => {
		:nuspec => "#{Projects[:memcached][:id]}.nuspec",
		:test => "#{Projects[:memcached][:test_dir]}.dll",
		:bin => "#{Projects[:memcached][:dir]}.dll"
	},
}

#-----------------------
# Commands
#-----------------------

Commands = {
	:nunit => File.join(Folders[:nunit], "nunit-console.exe"),
	:nuget => File.join(Folders[:nuget_bin], "NuGet.exe")
}

#safe function for creating output directories
def create_dir(dirName)
	if !File.directory?(dirName)
		FileUtils.mkdir(dirName) #creates the /build directory
	end
end

#Deletes a directory from the tree (to keep the build folder clean)
def flush_dir(dirName)
	if File.directory?(dirName)
		FileUtils.remove_dir(dirName, true)
	end
end