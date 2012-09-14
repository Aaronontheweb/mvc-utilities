$: << './'
require "rubygems"
require "bundler"
Bundler.setup

require 'albacore'
require 'version_bumper'

#-----------------------
# Local dependencies
#-----------------------
require File.expand_path(File.dirname(__FILE__)) + '/buildscripts/projects'
require File.expand_path(File.dirname(__FILE__)) + '/buildscripts/paths'

#-----------------------
# Environment variables
#-----------------------
@env_buildconfigname = "Release"

def env_buildversion
	bumper_version.to_s
end

#-----------------------
# Control flow
#-----------------------

desc "Bumps build number, runs build, and tests output"
task :default => [:bump_build_number, :msbuild, :test]

desc "Dry run of pushing to NuGet"
task :test_nuget => [:pack]

desc "Pushes a small revision to NuGet"
task :release_revision => [:bump_revision_number, :push]

desc "Pushes a minor release to NuGet"
task :release_minor => [:bump_minor_version_number, :push]

desc "Pushes a major release to NuGet"
task :release_minor => [:bump_major_version_number, :push]

#-----------------------
# Version Management
#-----------------------

assemblyinfo :assemblyinfo do |asm|
	assemblyInfoPath = File.join(Folders[:root], Files[:assembly_info])

	asm.input_file = assemblyInfoPath
	asm.output_file = assemblyInfoPath

	asm.version = env_buildversion
	asm.file_version = env_buildversion
end

desc "Increments the build number for the project"
task :bump_build_number do
	bumper_version.bump_build
	bumper_version.write(File.join(Folders[:root], Files[:version]))
end

desc "Increments the revision number for the project"
task :bump_revision_number do
	bumper_version.bump_revision
	bumper_version.write(File.join(Folders[:root], Files[:version]))
end

desc "Increments the minor version number for the project"
task :bump_minor_version_number do
	bumper_version.bump_minor
	bumper_version.write(File.join(Folders[:root], Files[:version]))
end

desc "Increments the major version number for the project"
task :bump_major_version_number do
	bumper_version.bump_major
	bumper_version.write(File.join(Folders[:root], Files[:version]))
end

#-----------------------
# MSBuild
#-----------------------
msbuild :msbuild => [:assemblyinfo] do |msb|
	msb.properties :configuration => @env_buildconfigname
	msb.targets :Clean, :Build #Does the equivalent of a "Rebuild Solution"
	msb.solution = File.join(Folders[:root], Files[:solution])
end

#-----------------------
# Tests
#-----------------------

nunit :test => [:msbuild] do |nunit|
	nunit.command = Commands[:nunit]
	nunit.options '/framework v4.0.30319'

	nunit.assemblies "#{Folders[:mvcutilities_tests]}/bin/#{@env_buildconfigname}/#{Files[:mvcutilities][:test]}", 
	"#{Folders[:memcached_tests]}/bin/#{@env_buildconfigname}/#{Files[:memcached][:test]}"
end

#-----------------------
# NuSpec
#-----------------------

task :nuspec_all => [:mvcutilities_nuspec, :bcrypt_nuspec, :azure_nuspec, :memcached_nuspec]

nuspec :mvcutilities_nuspec do |nuspec|
	nuspec.id = Projects[:mvcutilities][:id]
	nuspec.version = env_buildversion
	nuspec.authors = Projects[:mvcutilities][:authors]
	nuspec.description = Projects[:mvcutilities][:description]
	nuspec.title = Projects[:mvcutilities][:title]
	nuspec.language = Projects[:language]
	nuspec.licenseUrl = Projects[:licenseUrl]
	nuspec.projectUrl = Projects[:projectUrl]
	nuspec.output_file = File.join(Folders[:nuget_build], "#{Projects[:mvcutilities][:id]}-v#{env_buildversion}(#{@env_buildconfigname}).nuspec")
	nuspec.tags = "MVC ASP.NET MVC3 MVC4 Cache Hashing Encryption LowercaseRoute Routing Azure AppFabric Memcached"
end

nuspec :bcrypt_nuspec do |nuspec|
	nuspec.id = Projects[:bcrypt][:id]
	nuspec.version = env_buildversion
	nuspec.authors = Projects[:bcrypt][:authors]
	nuspec.description = Projects[:bcrypt][:description]
	nuspec.title = Projects[:bcrypt][:title]
	nuspec.language = Projects[:language]
	nuspec.licenseUrl = Projects[:licenseUrl]
	nuspec.projectUrl = Projects[:projectUrl]
	nuspec.dependency Projects[:mvcutilities][:id], env_buildversion
	nuspec.dependency Projects[:bcrypt][:dependencies][:bcrypt][:name], Projects[:bcrypt][:dependencies][:bcrypt][:version]
	nuspec.output_file = File.join(Folders[:nuget_build], "#{Projects[:bcrypt][:id]}-v#{env_buildversion}(#{@env_buildconfigname}).nuspec")
	nuspec.tags = "MVC ASP.NET MVC3 MVC4 Hashing Encryption BCrypt Blowfish"
end

nuspec :azure_nuspec do |nuspec|
	nuspec.id = Projects[:azure][:id]
	nuspec.version = env_buildversion
	nuspec.authors = Projects[:azure][:authors]
	nuspec.description = Projects[:azure][:description]
	nuspec.title = Projects[:azure][:title]
	nuspec.language = Projects[:language]
	nuspec.licenseUrl = Projects[:licenseUrl]
	nuspec.projectUrl = Projects[:projectUrl]
	nuspec.dependency Projects[:mvcutilities][:id], env_buildversion
	nuspec.dependency Projects[:azure][:dependencies][:AzureCaching][:name], Projects[:azure][:dependencies][:AzureCaching][:version]
	nuspec.output_file = File.join(Folders[:nuget_build], "#{Projects[:azure][:id]}-v#{env_buildversion}(#{@env_buildconfigname}).nuspec")
	nuspec.tags = "Caching Cache AppFabric AppFabricCache Memcache Azure MVC MVC4 MVC3"
end

nuspec :memcached_nuspec do |nuspec|
	nuspec.id = Projects[:memcached][:id]
	nuspec.version = env_buildversion
	nuspec.authors = Projects[:memcached][:authors]
	nuspec.description = Projects[:memcached][:description]
	nuspec.title = Projects[:memcached][:title]
	nuspec.language = Projects[:language]
	nuspec.licenseUrl = Projects[:licenseUrl]
	nuspec.projectUrl = Projects[:projectUrl]
	nuspec.dependency Projects[:mvcutilities][:id], env_buildversion
	nuspec.dependency Projects[:memcached][:dependencies][:enyim][:name], Projects[:memcached][:dependencies][:enyim][:version]
	nuspec.output_file = File.join(Folders[:nuget_build], "#{Projects[:memcached][:id]}-v#{env_buildversion}(#{@env_buildconfigname}).nuspec")
	nuspec.tags = "Caching Cache Memcache Memcached MVC MVC4 MVC3"
end

#-----------------------
# File Output
#-----------------------
desc "Sets the output directories according to our build environment"
task :set_output_folders do
	Folders[:mvcutilities_bin] = File.join(Folders[:root], Projects[:mvcutilities][:dir], 'bin', @env_buildconfigname)
	Folders[:bcrypt_bin] = File.join(Folders[:root], Projects[:bcrypt][:dir], 'bin', @env_buildconfigname)
	Folders[:azure_bin] = File.join(Folders[:root], Projects[:azure][:dir], 'bin', @env_buildconfigname)
	Folders[:memcached_bin] = File.join(Folders[:root], Projects[:memcached][:dir], 'bin', @env_buildconfigname)
end

desc "Creates all of the output folders we need for ILMerge / NuGet"
task :create_output_folders => :set_output_folders do
	create_dir(Folders[:out])
	create_dir(Folders[:nuget_build])

	create_dir(Folders[:mvcutilities_nuspec][:root])
	create_dir(Folders[:mvcutilities_nuspec][:lib])
	create_dir(Folders[:mvcutilities_nuspec][:net40])

	create_dir(Folders[:azure_nuspec][:root])
	create_dir(Folders[:azure_nuspec][:lib])
	create_dir(Folders[:azure_nuspec][:net40])

	create_dir(Folders[:bcrypt_nuspec][:root])
	create_dir(Folders[:bcrypt_nuspec][:lib])
	create_dir(Folders[:bcrypt_nuspec][:net40])

	create_dir(Folders[:memcached_nuspec][:root])
	create_dir(Folders[:memcached_nuspec][:lib])
	create_dir(Folders[:memcached_nuspec][:net40])
end

desc "Wipes out the build folder so we have a clean slate to work with"
task :clean_output_folders => :set_output_folders do
	puts "Flushing build folder..."
	flush_dir(Folders[:out])
end

task :prep_output => [:clean_output_folders, :create_output_folders]
task :mvcutilities_output => [:mvcutilities_static_output, :mvcutilities_net40_output]
task :bcrypt_output => [:bcrypt_static_output, :bcrypt_net40_output]
task :memcached_output => [:memcached_static_output, :memcached_net40_output]
task :azure_output => [:azure_static_output, :azure_net40_output]
task :all_output => [:mvcutilities_output, :bcrypt_output, :memcached_output, :azure_output]

output :mvcutilities_static_output => [:prep_output] do |out|
	out.from Folders[:root]
	out.to Folders[:mvcutilities_nuspec][:root]
	out.file Files[:readme]
	out.file Files[:license]
end

output :mvcutilities_net40_output => [:prep_output] do |out|
	out.from Folders[:mvcutilities_bin]
	create_dir(Folders[:mvcutilities_nuspec][:lib])
	out.to Folders[:mvcutilities_nuspec][:net40]
	out.file Files[:mvcutilities][:bin]
end

output :bcrypt_static_output => [:prep_output] do |out|
	out.from Folders[:root]
	out.to Folders[:bcrypt_nuspec][:root]
	out.file Files[:readme]
	out.file Files[:license]
end

output :bcrypt_net40_output => [:prep_output] do |out|
	out.from Folders[:bcrypt_bin]
	create_dir(Folders[:bcrypt_nuspec][:lib])
	out.to Folders[:bcrypt_nuspec][:net40]
	out.file Files[:bcrypt][:bin]
end

output :memcached_static_output => [:prep_output] do |out|
	out.from Folders[:root]
	out.to Folders[:memcached_nuspec][:root]
	out.file Files[:readme]
	out.file Files[:license]
end

output :memcached_net40_output => [:prep_output] do |out|
	out.from Folders[:memcached_bin]
	create_dir(Folders[:memcached_nuspec][:lib])
	out.to Folders[:memcached_nuspec][:net40]
	out.file Files[:memcached][:bin]
end

output :azure_static_output => [:prep_output] do |out|
	out.from Folders[:root]
	out.to Folders[:azure_nuspec][:root]
	out.file Files[:readme]
	out.file Files[:license]
end

output :azure_net40_output => [:prep_output] do |out|
	out.from Folders[:azure_bin]
	create_dir(Folders[:azure_nuspec][:lib])
	out.to Folders[:azure_nuspec][:net40]
	out.file Files[:azure][:bin]
end
#-----------------------
# NuGet Pack
#-----------------------

desc "Packs all of the MVC.Utilities solutions"
task :pack => [:mvcutilities_pack, :bcrypt_pack, :azure_pack, :memcached_pack]

nugetpack :mvcutilities_pack => [:test, :mvcutilities_output, :mvcutilities_nuspec] do |nuget|
	nuget.command = Commands[:nuget]
	nuget.nuspec = File.join(Folders[:nuget_build], "#{Projects[:mvcutilities][:id]}-v#{env_buildversion}(#{@env_buildconfigname}).nuspec")
	nuget.base_folder = Folders[:mvcutilities_nuspec][:root]
	nuget.output = Folders[:nuget_build]
end

nugetpack :bcrypt_pack => [:test, :bcrypt_output, :bcrypt_nuspec] do |nuget|
	nuget.command = Commands[:nuget]
	nuget.nuspec = File.join(Folders[:nuget_build], "#{Projects[:bcrypt][:id]}-v#{env_buildversion}(#{@env_buildconfigname}).nuspec")
	nuget.base_folder = Folders[:bcrypt_nuspec][:root]
	nuget.output = Folders[:nuget_build]
end

nugetpack :azure_pack => [:test, :azure_output, :azure_nuspec] do |nuget|
	nuget.command = Commands[:nuget]
	nuget.nuspec = File.join(Folders[:nuget_build], "#{Projects[:azure][:id]}-v#{env_buildversion}(#{@env_buildconfigname}).nuspec")
	nuget.base_folder = Folders[:azure_nuspec][:root]
	nuget.output = Folders[:nuget_build]
end

nugetpack :memcached_pack => [:test, :memcached_output, :memcached_nuspec] do |nuget|
	nuget.command = Commands[:nuget]
	nuget.nuspec = File.join(Folders[:nuget_build], "#{Projects[:memcached][:id]}-v#{env_buildversion}(#{@env_buildconfigname}).nuspec")
	nuget.base_folder = Folders[:memcached_nuspec][:root]
	nuget.output = Folders[:nuget_build]
end

#-----------------------
# NuGet Push
#-----------------------

desc "Pushes all of our NuGet packages directly to nuget.org"
task :push => [:mvcutilities_push, :bcrypt_push, :azure_push, :memcached_push]

nugetpush :mvcutilities_push => [:pack] do |nuget|
    nuget.command = Commands[:nuget]
    nuget.package = File.join(Folders[:nuget_build], "#{Projects[:mvcutilities][:id]}.#{env_buildversion}.nupkg").gsub("/","\\")
end

nugetpush :bcrypt_push => [:pack] do |nuget|
    nuget.command = Commands[:nuget]
    nuget.package = File.join(Folders[:nuget_build], "#{Projects[:bcrypt][:id]}.#{env_buildversion}.nupkg").gsub("/","\\")
end

nugetpush :azure_push => [:pack] do |nuget|
    nuget.command = Commands[:nuget]
    nuget.package = File.join(Folders[:nuget_build], "#{Projects[:azure][:id]}.#{env_buildversion}.nupkg").gsub("/","\\")
end

nugetpush :memcached_push => [:pack] do |nuget|
    nuget.command = Commands[:nuget]
    nuget.package = File.join(Folders[:nuget_build], "#{Projects[:memcached][:id]}.#{env_buildversion}.nupkg").gsub("/","\\")
end