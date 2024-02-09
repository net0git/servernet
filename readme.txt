datos a conciderar=================================================
1) el servidor es de tipo web api y se conecta a los servicios de FIREBASE
2) la base de datos que utiliza el servidor se basa en el siguiente formato json
	[
		"id_game":{
				nameGame:"nombre del juego"
				description:"descripcion del juego"
				url:"url de la imagen del juego"
		},
		.
		.
		.
	]
3)la coneccion de la base de datos esta en publico, entonces deberia funcionar de manera automatica 
   una vez que se inicialice el servidor, sino es asi, crear una cuenta en firebase/crear un proyecto/ inyectar las credenciales al servidor

 IFirebaseConfig config = new FirebaseConfig
 {
     AuthSecret = "<REEMPLAZAR CREDENCIAL>",
     BasePath = "<REEMPLAZAR CREDENCIAL>"
 };
	

INSTALADORES//=====================================================================================
dentro de la linea de comnandos instalar
1)npm install -g firebase-tools
2)npm install firebase-admin

