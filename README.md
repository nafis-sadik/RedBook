
# Redbook

Redbook is a free, open-source inventory management and invoicing solution designed to streamline your business operations. The web API is built using __.NET__, while the frontend is developed with __Angular__. To ensure secure access, Redbook employs __authentik__ for __OAuth2 and OIDC__.

For efficient data handling, __Redis__ is utilized for server-side caching, enhancing the performance of the application. Initially, we opted for __MS SQL Server__ as our database during development; however, we plan to transition to __PostgreSQL__ as it is a free, open-source solution that allows for easy self-hosting nature complements Redbook more effectively than MS SQL Server.

Explore the features of Redbook and see how it can simplify your inventory management and invoicing needs!
## Installation & Run Locally

### Install Docker
* [Windows](https://docs.docker.com/desktop/setup/install/windows-install/)
* [linux](https://docs.docker.com/desktop/setup/install/linux/)
* [Mac](https://docs.docker.com/desktop/setup/install/mac-install/)
### Run Authentik with the docker compose
* Open the folder in terminal
    `src > authentik`
* Duplicate the file `.env` and name it `.dev.env`
* Populate your environment variables
* Run the following command
    ```
    docker compose --env-file authentik.env up -d
    ```
### Angular
* Open the folder in terminal
    `src > Frontend`
* Run `npm i` to install dependencies
* Run the command `ng serve` or press `Ctrl + F5` 
We included the `.vscode` folder with the repository (although that's not the convention) so that you can use  `Ctrl + F5` or click `Run > Start Debugging` or `Run > Run Without Debugging` features of `vs code` out of the box, without any configurations on your part. You can edit the configurations in it, if you want and you know what you are doing.