job("Build and push Docker (backend)") {
    startOn {
        gitPush {
            anyBranchMatching {
                +"main"
            }
        }
    }
    host("Build artifacts and a Docker image (backend)") {
       dockerBuildPush {

            file = "./quotes-backend/Dockerfile"
            labels["vendor"] = "DigitalIndividuals"

            tags {
                +"cfr-r.divsphere.net/quote-backend:latest"
            }
        }
    }
}