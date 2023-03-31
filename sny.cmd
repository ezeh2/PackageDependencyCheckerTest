

# The token you will need to use for snyk auth is the one in your account settings here, under Auth Token: 
# https://app.snyk.io/account
# snyk auth <token>

# token is save here
# C:\Users\ereutlinger\.config\configstore\snyk.json


cd C:\Work\PackageDependencyCheckerTest\OblivionAngularWebAppJavaScriptReference
snyk monitor

cd C:\Work\PackageDependencyCheckerTest\OblivionAppPackageConfigNet48
snyk monitor


cd C:\Work\PackageDependencyCheckerTest\OblivionAppPackageReferenceDotNet60
snyk monitor


cd C:\Work\PackageDependencyCheckerTest\OblivionAppPackageReferenceNet48
snyk monitor


cd C:\Work\PackageDependencyCheckerTest\OblivionAppPackageReferenceNodeJs
snyk monitor



cd C:\Work\PackageDependencyCheckerTest\laps_compare
snyk code test


# C:\Work\PackageDependencyCheckerTest\laps_compare>snyk code test
# Testing C:\Work\PackageDependencyCheckerTest\laps_compare ...
# 
# ✔ Test completed
# 
# Organization:      ezeh2
# Test type:         Static code analysis
# Project path:      C:\Work\PackageDependencyCheckerTest\laps_compare
# 
# Summary:
# ✔ Awesome! No issues were found.



cd C:\Work\PackageDependencyCheckerTest\CyberSecurityWebApplication\CyberSecurity3WebApplication
snyk code test


