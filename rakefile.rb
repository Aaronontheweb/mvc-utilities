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

nunit :nunit_tests => [:msbuild] do |nunit|
	nunit.command = Commands[:nunit]
	nunit.options '/framework v4.0.30319'

	nunit.assemblies "#{Folders[:mvcutilities_tests]}/bin/#{@env_buildconfigname}/#{Files[:mvcutilities][:test]}", 
	"#{Folders[:memcached_tests]}/bin/#{@env_buildconfigname}/#{Files[:memcached][:test]}"
end

#-----------------------
# NuSpec
#-----------------------

#-----------------------
# File Output
#-----------------------
desc "Sets the output directories according to our build environment"
task :set_output_directories do
	Folders[:mvcutilities_bin] = File.join(Folders[:root], Projects[:mvcutilities][:dir], 'bin', @env_buildconfigname),
	Folders[:bcrypt_bin] = File.join(Folders[:root], Projects[:bcrypt][:dir], 'bin', @env_buildconfigname),
	Folders[:azure_bin] = File.join(Folders[:root], Projects[:azure][:dir], 'bin', @env_buildconfigname),
	Folders[:memcached_bin] = File.join(Folders[:root], Projects[:memcached][:dir], 'bin', @env_buildconfigname)
end

#-----------------------
# NuGet Packages
#-----------------------