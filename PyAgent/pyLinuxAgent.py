import apt

# Create an instance of the apt cache
cache = apt.Cache()

# Update the package lists
cache.update()

# Open the cache
cache.open()

# Get a list of installed packages
installed_packages = [pkg for pkg in cache if pkg.is_installed]

# Print the name and version of each installed package
for package in installed_packages:
    print(f"Name: {package.name}")
    print(f"Version: {package.installed.version}")
    print("------------------------")