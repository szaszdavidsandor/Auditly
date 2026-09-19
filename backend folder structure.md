backend/
    src/                            # <--- The source code projects
        Presentation/               # Presentation Layer (Controllers, Middlewares)
            Controller/             # handles requests and makes the responses
            Dto/                    #

        Domain/                     # Domain Layer (Business rules)
            Model/                  # Entities
            Interface/              # a contract beetwen layers(for rules and communication)
            Enum/                   # predetermined values

        Application/                # Application Layer (Business logic(use case))
            Interface/              # -||-
            UseCase/                # business logic(2FA, generate QR code, ect.)
        
        Infrastructure/             # infrastructure Layer (Database access, migrations, and other external services)
            Repository/             # Database connection
            Migration/              # Database Migration files
            Authentication/         # jwt or session, ect.


    docs/                           # <--- BACKEND'S OWN DOCUMENTATION
        plans/
            presentation/         
                presentation.md   

            domain/               
                domain.md         

            application/          
                application.md    

            infrastructure/       
                infrastructure.md 