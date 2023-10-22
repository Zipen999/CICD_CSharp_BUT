def main(ctx):
  commit_msg = ctx.build.message.lower()
  if "README.md" in commit_msg or "[no_ci]" in commit_msg:
    return vide()

  if "master" in ctx.build.branch :
    return [
      ci(ctx),
      cd(ctx),
    ]
  else :
    if "[cd]" in commit_msg:
      return [
        ciAlt(ctx),
        cd(ctx),
      ]
    else:
      return ciAlt(ctx)


def vide():
  return {
    "kind": "pipeline",
    "name": "Vide",
    "steps": []
  }
  
def ci(ctx):
  CI = {
    "kind": "pipeline",
    "name": "CI_Starlark",
    "steps": [
      {
        "name": "build",
        "image": "mcr.microsoft.com/dotnet/sdk:7.0",
        "commands": [
            "cd Sources",
            "dotnet restore OpenLibraryWS_Wrapper.sln",
            "dotnet build OpenLibraryWS_Wrapper.sln -c Release --no-restore",
            "dotnet publish OpenLibraryWS_Wrapper.sln -c Release --no-restore -o CI_PROJECT_DIR/build/release",
        ]
      },
      {
        "name": "tests",
        "image": "mcr.microsoft.com/dotnet/sdk:7.0",
        "commands": [
            "cd Sources/",
            "dotnet restore OpenLibraryWS_Wrapper.sln", 
            "dotnet test OpenLibraryWS_Wrapper.sln --no-restore",
        ],
        "depends_on": [
          "build"
        ]
      },
      {
        "name": "code-analysis",
        "image": "hub.codefirst.iut.uca.fr/marc.chevaldonne/codefirst-dronesonarplugin-dotnet7",
        "commands": [
            "cd Sources",
            "dotnet restore OpenLibraryWS_Wrapper.sln",
            "dotnet sonarscanner begin /k:LIVET_CICD_OpenLibraryWS_Wrapper /d:sonar.host.url=$${PLUGIN_SONAR_HOST} /d:sonar.coverageReportPaths=\"coveragereport/SonarQube.xml\" /d:sonar.coverage.exclusions=\"Tests/**,DbManager/**,Client/**\" /d:sonar.login=$${PLUGIN_SONAR_TOKEN}",
            "dotnet build OpenLibraryWS_Wrapper.sln -c Release --no-restore",
            "dotnet publish OpenLibraryWS_Wrapper.sln -c Release --no-restore -o CI_PROJECT_DIR/build/release",
            "dotnet test OpenLibraryWS_Wrapper.sln --logger trx --no-restore /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura --collect \"XPlat Code Coverage\"",
            "reportgenerator -reports:\"**/coverage.cobertura.xml\" -reporttypes:SonarQube -targetdir:\"coveragereport\"",
            "dotnet publish OpenLibraryWS_Wrapper.sln -c Release --no-restore -o CI_PROJECT_DIR/build/release",
            "dotnet sonarscanner end /d:sonar.login=$${PLUGIN_SONAR_TOKEN}",
        ],
        "secrets": [
          "SECRET_SONAR_LOGIN"
        ],
        "settings": {
          "sonar_host": "https://codefirst.iut.uca.fr/sonar/",
          "sonar_token": {"from_secret": "SECRET_SONAR_LOGIN"},
        },
        "depends_on": [
          "tests"
        ]
      },
      {
        "name": "generate-doxygen",
        "image": "hub.codefirst.iut.uca.fr/thomas.bellembois/codefirst-docdeployer",
        "failure": "ignore",
        "volumes": [
            {
                "name": "docs",
                "path": "/docs"
            }
        ],
        "commands": [
            "/entrypoint.sh"
      ],
      "depends_on": [
        "code-analysis"
      ]
      }
    ],
    "volumes": [
        {
          "name": "docs",
          "temp": {}
        }
    ]
  }
  return CI

def ciAlt(ctx):
  CI = {
    "kind": "pipeline",
    "name": "CI_Starlark",
    "steps": [
      {
        "name": "build",
        "image": "mcr.microsoft.com/dotnet/sdk:7.0",
        "commands": [
            "cd Sources",
            "dotnet restore OpenLibraryWS_Wrapper.sln",
            "dotnet build OpenLibraryWS_Wrapper.sln -c Release --no-restore",
            "dotnet publish OpenLibraryWS_Wrapper.sln -c Release --no-restore -o CI_PROJECT_DIR/build/release",
        ]
      },
      {
        "name": "tests",
        "image": "mcr.microsoft.com/dotnet/sdk:7.0",
        "commands": [
            "cd Sources/",
            "dotnet restore OpenLibraryWS_Wrapper.sln", 
            "dotnet test OpenLibraryWS_Wrapper.sln --no-restore",
        ],
        "depends_on": [
          "build"
        ]
      }    
    ]
  }
  return CI


def cd(ctx):
  CD = {
    "kind": "pipeline",
    "name": "CD_Starlark",
    "steps": [
      {
        "name": "verif-dockerfile",
        "image": "hadolint/hadolint:latest-alpine",
        "commands": [
            "hadolint Sources/OpenLibraryWrapper/Dockerfile"
        ]
      },
      {
        "name": "docker-build",
        "image": "plugins/docker",
        "settings": {
          "dockerfile": "Sources/OpenLibraryWrapper/Dockerfile",
          "context": "Sources/",
          "registry": "hub.codefirst.iut.uca.fr",
          "repo": "hub.codefirst.iut.uca.fr/hugo.livet/cicd_openlibraryws_wrapper",
          "username": {"from_secret": "secret-registry-username"},
          "password": {"from_secret": "secret-registry-password"}
        },
        "depends_on": [
          "verif-dockerfile"
        ]
      },
      {
        "name": "deploy-container-mysql",
        "image": "hub.codefirst.iut.uca.fr/thomas.bellembois/codefirst-dockerproxy-clientdrone:latest",
        "environment": {
        "IMAGENAME": "mariadb:10",
        "CONTAINERNAME": "mysql",
        "COMMAND": "create",
        "PRIVATE": "true",
        "CODEFIRST_CLIENTDRONE_ENV_MARIADB_ROOT_PASSWORD": {"from_secret": "db_root_password"},
        "CODEFIRST_CLIENTDRONE_ENV_MARIADB_DATABASE": {"from_secret": "db_database"},
        "CODEFIRST_CLIENTDRONE_ENV_MARIADB_USER": {"from_secret": "db_user"},
        "CODEFIRST_CLIENTDRONE_ENV_MARIADB_PASSWORD": {"from_secret": "db_password"}
        },
        "depends_on": [
        "docker-build"
        ]
      }
    ]
  }
  
  env_type_data = "BDD"
  if "[database]" in ctx.build.message.lower():
      env_type_data = "BDD"
  elif "[stub]" in ctx.build.message.lower():
      env_type_data = "STUB"
  elif "[wrapper]" in ctx.build.message.lower():
      env_type_data = "API"
      
  CD["steps"].append({
      "name": "deploy-container",
      "image": "hub.codefirst.iut.uca.fr/thomas.bellembois/codefirst-dockerproxy-clientdrone:latest",
      "environment": {
      "CODEFIRST_CLIENTDRONE_ENV_TYPE_DATA": env_type_data,
      "CODEFIRST_CLIENTDRONE_ENV_HOST_DB": "hugolivet-mysql",
      "CODEFIRST_CLIENTDRONE_ENV_PORT_DB": "3306",
      "CODEFIRST_CLIENTDRONE_ENV_NAME_DB": {"from_secret": "db_database"},
      "CODEFIRST_CLIENTDRONE_ENV_USER_DB": {"from_secret": "db_user"},
      "CODEFIRST_CLIENTDRONE_ENV_PSWD_DB": {"from_secret": "db_password"},
      "IMAGENAME": "hub.codefirst.iut.uca.fr/hugo.livet/cicd_openlibraryws_wrapper:latest",
      "CONTAINERNAME": "hugo_livet_cicd_"+env_type_data,
      "COMMAND": "create",
      "OVERWRITE": "true",
      },
      "depends_on": [
      "deploy-container-mysql"
      ]
  })
  return CD
