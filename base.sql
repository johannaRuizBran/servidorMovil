--CREATE DATABASE mantenimiento;
--ELIMINAR
--USE Quiz
--DROP DATABASE mantenimiento
--use master;
--use mantenimiento;


CREATE TABLE usuario(
	nombreUsuario	VARCHAR(100) PRIMARY KEY,
	contrasena	VARCHAR(100) not null,
	nombre VARCHAR(100) not null,
	apellido1 VARCHAR(100) not null,
	apellido2 VARCHAR(100) not null,
	correo VARCHAR(100) not null,
	telefono CHAR(8) not null,
	rol VARCHAR(50) null,
	activo CHAR(2) not null,
	CONSTRAINT CK_usuario_correo	CHECK(correo like '[A-z]%@[A-z]%.[A-z]%'),
	CONSTRAINT CK_usuario_telefono CHECK(telefono like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	CONSTRAINT CK_usuario_activo CHECK(activo in ('Si','No')),
	CONSTRAINT CK_usuario_rol CHECK(rol in ('Administrador','Profesor','Tecnico','Operador'))
);



CREATE TABLE computadora(
	codigo VARCHAR(100) PRIMARY KEY,
	posicionX FLOAT not null,
	posicionY FLOAT not null,
	establecimiento VARCHAR(100) not null,
	--CONSTRAINT CK_computadora_establecimiento CHECK(establecimiento in ('LAB-01','LAB-02','Miniauditorio','Moviles','SIRZEE')),
);

CREATE TABLE reporte(
	id INT IDENTITY(1,1) PRIMARY KEY,
	estadoReporte VARCHAR(100) not null,
	prioridadReporte VARCHAR(100) null,
	fechaReporte DATE DEFAULT GETDATE(),
	fechaFinalizacion DATE not null,
	descripcion VARCHAR(500) not null,
	establecimiento VARCHAR(100) not null,
	CONSTRAINT CK_reporte_estadoReporte CHECK(estadoReporte in ('Cancelado','Finalizado','enProceso','conPrioridad','nuevo','informacion')),
	CONSTRAINT CK_reporte_prioridadReporte CHECK(prioridadReporte in ('Alto','Medio','Bajo')),
	CONSTRAINT CK_reporte_establecimiento CHECK(establecimiento in ('LAB-01','LAB-02','Miniauditorio','Moviles','SIRZEE'))
);


CREATE TABLE reporte_informacion(
	idReporte INT PRIMARY KEY,	
	observacion VARCHAR(500) NOT NULL
	CONSTRAINT FK_idreporte_reporte_informacion FOREIGN KEY(idReporte) REFERENCES reporte ON DELETE CASCADE ON UPDATE CASCADE	
);


CREATE TABLE usuariosReporte(
	idReporte INT not null,
	idUsuario VARCHAR(100) not null,
	rol VARCHAR(50) not null,
	CONSTRAINT PK_usuariosReporte_idReporte_idUsuarioTecnico PRIMARY KEY(idReporte,idUsuario),
	CONSTRAINT FK_usuariosReporte_idReporte FOREIGN KEY(idReporte) REFERENCES reporte ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT CK_usuariosReporte_rol CHECK(rol in ('Tecnico','Usuario')),
	CONSTRAINT FK_usuariosReporte_idUsuarioTenico FOREIGN KEY(idUsuario) REFERENCES usuario ON DELETE CASCADE ON UPDATE CASCADE
);



CREATE TABLE detalleReporte(
	idReporte INT not null,
	codigoComputadora VARCHAR(100) not null,
	estadoComputadora VARCHAR(100) not null,
	descripcion VARCHAR(100),
	CONSTRAINT PK_detalleReporte_idReporte_codigoComputadora PRIMARY KEY(idReporte,codigoComputadora),
	CONSTRAINT FK_detalleReporte_idReporte FOREIGN KEY(idReporte)REFERENCES reporte ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_detalleReporte_codigoComputadora FOREIGN KEY(codigoComputadora)REFERENCES computadora ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT CK_detalleReporte_estadoComputadora CHECK (estadoComputadora in ('Rojo','Amarillo','Verde','Gris'))
);

GO


--------------- procedure


--FUNCIONES USUARIO

CREATE PROCEDURE insertarUsuario(
									@nombreUsuarioVar VARCHAR(100), 
									@contrasenaVar VARCHAR(100),
									@nombreVar VARCHAR(100),
									@apellido1Var VARCHAR(100),
									@apellido2Var VARCHAR(100),
									@correoVar VARCHAR(100), 
									@telefonoVar VARCHAR(100),
									@rolVar	VARCHAR(100),
									@permisoAdmin VARCHAR(10)
								)
as
BEGIN

	if @permisoAdmin = 'N'
		BEGIN
			 	INSERT INTO usuario(nombreUsuario,contrasena,nombre,apellido1,apellido2,correo,telefono,rol,activo)
						VALUES(@nombreUsuarioVar,@contrasenaVar, @nombreVar,@apellido1Var,@apellido2Var,@correoVar,@telefonoVar,@rolVar,'No');
		END
	ELSE
		BEGIN
			 	INSERT INTO usuario(nombreUsuario,contrasena,nombre,apellido1,apellido2,correo,telefono,rol,activo)
				VALUES(@nombreUsuarioVar,@contrasenaVar, @nombreVar,@apellido1Var,@apellido2Var,@correoVar,@telefonoVar,@rolVar,'Si');
		END	

END;

GO

--eliminar usuario
CREATE PROCEDURE eliminarUsuario(@nombreUsuarioVar VARCHAR(100))
as
BEGIN
	DELETE FROM usuario WHERE nombreUsuario = @nombreUsuarioVar;
END;

GO

--modificar informacion de usuario 
CREATE PROCEDURE actualizarUsuario(
									@nombreUsuarioOLD VARCHAR(100), 
									@nombreUsuarioVar VARCHAR(100), 
									@contrasenaVar VARCHAR(100),
									@nombreVar VARCHAR(100),
									@apellido1Var VARCHAR(100),
									@apellido2Var VARCHAR(100),
									@correoVar VARCHAR(100), 
									@telefonoVar VARCHAR(100), 
									@rolVar VARCHAR(50),
									@permisoAdmin VARCHAR(1)
								)
as
BEGIN

	if @permisoAdmin = 'N'
		BEGIN
			 		UPDATE usuario
						SET 
							nombreUsuario = @nombreUsuarioVar,
							contrasena = @contrasenaVar,
							nombre = @nombreVar,
							apellido1 = @apellido1Var,
							apellido2 = @apellido2Var,
							correo = @correoVar,
							telefono = @telefonoVar
						WHERE nombreUsuario = @nombreUsuarioOLD;
		END
	ELSE
		BEGIN
			 		UPDATE usuario
						SET 
							nombreUsuario = @nombreUsuarioVar,
							contrasena = @contrasenaVar,
							nombre = @nombreVar,
							apellido1 = @apellido1Var,
							apellido2 = @apellido2Var,
							correo = @correoVar,
							telefono = @telefonoVar,
							rol = @rolVar
						WHERE nombreUsuario = @nombreUsuarioOLD;
		END	

END;



GO


CREATE PROCEDURE activarUsuario(
									@nombreUsuarioVar VARCHAR(100)
								)
as
BEGIN
	UPDATE usuario
	SET 
		activo = 'Si'
	WHERE nombreUsuario = @nombreUsuarioVar;
END;


GO



--FUNCIONES REPORTE


--crear reporte

CREATE PROCEDURE crearReporte(
									@estadoReporteVar VARCHAR(100),									
									@fechaFinalizacionVar DATE, 
									@descripcionVar VARCHAR(500),
									@establecimientoVar VARCHAR(100),
									@idUsuarioVar	VARCHAR(100)
								)
as
DECLARE
	@ultimoRegistro INT
BEGIN
	INSERT INTO reporte(estadoReporte,fechaFinalizacion,descripcion,establecimiento)
		VALUES(@estadoReporteVar,@fechaFinalizacionVar,@descripcionVar,@establecimientoVar);

	set @ultimoRegistro = @@IDENTITY;

	INSERT INTO usuariosReporte(idReporte,idUsuario,rol)
		VALUES(@ultimoRegistro,@idUsuarioVar,'Usuario');
END;


GO

CREATE PROCEDURE eliminarReporte(@idReporteVar INT)
as
BEGIN
	DELETE FROM reporte WHERE id = @idReporteVar;
END;


GO

--modificar reporte
CREATE PROCEDURE modificarReporte(
									@idVar INT,
									@estadoReporteVar VARCHAR(100),
									@prioridadReporteVar VARCHAR(100),
									@fechaFinalizacionVar DATE, 
									@descripcionVar VARCHAR(500),
									@establecimientoVar VARCHAR(100) 
								)
as
BEGIN
	UPDATE reporte
	SET 
		estadoReporte = @estadoReporteVar,
		prioridadReporte = @prioridadReporteVar,
		fechaFinalizacion = @fechaFinalizacionVar,
		descripcion = @descripcionVar,
		establecimiento = @establecimientoVar
	WHERE id = @idVar
END;
GO

--modificar reporte
CREATE PROCEDURE modificarDetalleReporte(
									@idReporte INT ,
									@codigoComputadora VARCHAR(100),
									@estadoComputadora VARCHAR(100),
									@descripcion VARCHAR(100)
								)
as
BEGIN
	UPDATE detalleReporte
	SET 
		estadoComputadora = @estadoComputadora ,
		descripcion = @descripcion 
		
	WHERE idReporte = @idReporte and codigoComputadora = @codigoComputadora;
END;
GO

--modificar fecha finalizacion y prioridad de reporte
CREATE PROCEDURE modificarFechaYPrioridadReporte(
									@idReporteVar INT,
									@prioridadReporteVar VARCHAR(100),
									@fechaFinalizacionVar DATE							
								)
as
BEGIN
	UPDATE reporte
	SET 
		prioridadReporte = @prioridadReporteVar,
		fechaFinalizacion = @fechaFinalizacionVar
	WHERE id = @idReporteVar;
END;


GO

--agregar nueva informacion del reporte
CREATE PROCEDURE agregarDescripcionReporte(
									@idReporteVar INT,
									@descripcionNEW VARCHAR(500)						
								)
as
DECLARE
	@descripcionOLD VARCHAR(500),
	@descripcionReporte VARCHAR(500)

BEGIN
	set @descripcionOLD =  (select descripcion from reporte where id = @idReporteVar);
	set @descripcionReporte = @descripcionOLD + '   Nueva informacion: '+ @descripcionNEW;
	UPDATE reporte
	SET 
		descripcion = @descripcionReporte
	WHERE id = @idReporteVar;
END;


GO

--FUNCIONES USUARIOS REPORTE


CREATE PROCEDURE obtenerInformacionFaltante(
								@idReporte INT
							)
as
BEGIN
	select * from reporte_informacion where idReporte= @idReporte;
END;

go



--insertar
CREATE PROCEDURE insertarUsuariosReporte(
									@idReporteVar INT, 
									@idUsuarioVar VARCHAR(100),
									@rolVar VARCHAR(100)
								)
as
BEGIN
	INSERT INTO usuariosReporte(idReporte,idUsuario,rol)
		VALUES(@idReporteVar,@idUsuarioVar,@rolVar);
END;

GO

--eliminar
CREATE PROCEDURE eliminarUsuarioReporte(
									@idReporteVar INT, 
									@idUsuarioVar VARCHAR(100)
								)
as
BEGIN
	DELETE FROM usuariosReporte WHERE idReporte = @idReporteVar and idUsuario = @idUsuarioVar;
END;

GO

CREATE PROCEDURE crearEnlaceLabreporte(	
								@idUsuarioVar VARCHAR(100))
as
DECLARE
	@idReporte int
		
BEGIN
	set @idReporte = (SELECT TOP 1 id FROM reporte inner join usuariosReporte 
		on (usuariosReporte.idUsuario = @idUsuarioVar) ORDER BY id DESC)

	exec insertarDetalleReporte @idReporte
END;

go 

select * from detalleReporte

--crear detalleReporte
CREATE PROCEDURE insertarDetalleReporte(
									@idReporteVar INT
								)
as
DECLARE
	@establecimiento VARCHAR(50),
	@codigoCompu VARCHAR(100)
BEGIN
	set @establecimiento = (select establecimiento from reporte where id = @idReporteVar);
	DECLARE
		cursorComputadoras CURSOR FOR
								(SELECT codigo FROM computadora WHERE establecimiento=@establecimiento);

	OPEN cursorComputadoras;
	FETCH NEXT FROM cursorComputadoras into @codigoCompu;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		insert into detalleReporte(idReporte,codigoComputadora,estadoComputadora)VALUES(@idReporteVar,@codigoCompu,'Rojo');	
		FETCH NEXT FROM cursorComputadoras into @codigoCompu;

	END;
	close cursorComputadoras;
	deallocate cursorComputadoras;

END;

GO


--FUNCIONES DE SELECT


--lista de todos los reportes por medio de filtro (el estado del reporte "cancelado","en proceso" ......)
CREATE PROCEDURE selectReportesFiltro(
									@filtro VARCHAR(100)
								)
as
BEGIN
	RETURN SELECT * FROM reporte WHERE estadoReporte = @filtro;
END;


GO


--lista el estados de las computadoras segun el reporte
CREATE PROCEDURE selectEstadosComputadoras(
									@idReporteVar INT
								)
as
BEGIN
	select estadoComputadora from detalleReporte as d inner join computadora as c on(d.codigoComputadora = c.codigo)
	where d.idReporte = @idReporteVar
	order by c.posicionX;
	
END;


GO



CREATE PROCEDURE selectReportesEspecificos(@tipoReporte varchar(100))
AS 
BEGIN
	 select r.id,r.estadoReporte,r.prioridadReporte,r.fechaReporte,r.fechaFinalizacion,r.descripcion,
	 r.establecimiento,u.nombreUsuario,(u.nombre + ' ' + u.apellido1 + ' ' +  u.apellido2) as nombre from reporte as r 
	 inner join usuariosReporte as ur on(r.id = ur.idReporte) 
	 inner join usuario as u on(ur.idUsuario = u.nombreUsuario) 
	 where r.estadoReporte = @tipoReporte and ur.rol = 'Usuario' 
END;


--lista de reportes por hacer de un tecnico

go


CREATE PROCEDURE selectListaDeReportesTecnico(@nombreU varchar(100))
AS 
BEGIN		

	select reporte.id,reporte.estadoReporte, reporte.prioridadReporte, reporte.fechaReporte,
	reporte.fechaFinalizacion, reporte.descripcion, reporte.establecimiento,  
	usuario.nombreUsuario, (usuario.nombre + ' ' + usuario.apellido1 + ' ' +  usuario.apellido2) as nombre		
	
	 from usuario inner join usuariosReporte on(usuariosReporte.idUsuario = usuario.nombreUsuario)
	 inner join reporte on (reporte.id= usuariosReporte.idReporte and usuariosReporte.idUsuario= @nombreU AND reporte.estadoReporte !='Finalizado') 

END;

go
--lista de reportes de un usuario


CREATE PROCEDURE selectListaDeReportes(@personaVar varchar(100))
AS 
BEGIN		

	select reporte.id,reporte.estadoReporte, reporte.prioridadReporte, reporte.fechaReporte,
	reporte.fechaFinalizacion, reporte.descripcion, reporte.establecimiento,  
	usuario.nombreUsuario, (usuario.nombre + ' ' + usuario.apellido1 + ' ' +  usuario.apellido2) as nombre		
	
	 from usuario inner join usuariosReporte on(usuariosReporte.idUsuario = usuario.nombreUsuario)
	 inner join reporte on (reporte.id= usuariosReporte.idReporte and usuariosReporte.idUsuario=  @personaVar) 
END;

go

CREATE PROCEDURE selectReporte(@idVar varchar(100))
AS 
BEGIN		

	select reporte.id,reporte.estadoReporte, reporte.prioridadReporte, reporte.fechaReporte,
	reporte.fechaFinalizacion, reporte.descripcion, reporte.establecimiento,  
	usuario.nombreUsuario, (usuario.nombre + ' ' + usuario.apellido1 + ' ' +  usuario.apellido2) as nombre		
	
	 from usuario inner join usuariosReporte on(usuariosReporte.idUsuario = usuario.nombreUsuario)
	 inner join reporte on (reporte.id= usuariosReporte.idReporte and usuariosReporte.idReporte=  @idVar) 
END;

--tecnicos de un reporte
GO


CREATE PROCEDURE selectTecnicosReporte
AS 
BEGIN
	SELECT * FROM usuariosReporte inner join reporte on reporte.id = usuariosReporte.idReporte
END;


GO



--devuelve los usuarios de un rol en espec�fico o a todos los usuarios
CREATE PROCEDURE listaDeUsuario (@tipoU varchar(100))
AS 
BEGIN		
	if @tipoU = 'N'
		BEGIN
			 SELECT * FROM usuario WHERE activo='Si'
		END
	ELSE
		BEGIN
			 SELECT * FROM usuario WHERE rol= @tipoU and activo='Si'
		END		
	return
END;


GO


-------- nuevo ------------------


CREATE PROCEDURE selectReportesPrioritariosFiltro(@tipoPrioridad varchar(100))
AS 
BEGIN

	if @tipoPrioridad = 'fecha'
		BEGIN
			select r.id,r.estadoReporte,r.prioridadReporte,r.fechaReporte,r.fechaFinalizacion,r.descripcion,
			 r.establecimiento,u.nombreUsuario,(u.nombre + ' ' + u.apellido1 + ' ' +  u.apellido2) as nombre from reporte as r 
			 inner join usuariosReporte as ur on(r.id = ur.idReporte) 
			 inner join usuario as u on(ur.idUsuario = u.nombreUsuario) 
			 where r.estadoReporte = 'conPrioridad' and ur.rol = 'Usuario' order by r.fechaFinalizacion
		END
	ELSE
		BEGIN
			 select r.id,r.estadoReporte,r.prioridadReporte,r.fechaReporte,r.fechaFinalizacion,r.descripcion,
			 r.establecimiento,u.nombreUsuario,(u.nombre + ' ' + u.apellido1 + ' ' +  u.apellido2) as nombre from reporte as r 
			 inner join usuariosReporte as ur on(r.id = ur.idReporte) 
			 inner join usuario as u on(ur.idUsuario = u.nombreUsuario) 
			 where r.estadoReporte = 'conPrioridad' and ur.rol = 'Usuario' 
			 order by 
					CASE r.prioridadReporte
					  WHEN 'Alto' THEN 1
					  WHEN 'Medio' THEN 2
					  WHEN 'Bajo' THEN 3
				   END;
		END;		
END;


go


CREATE PROCEDURE actualizarInformacionReporte(@idReporte int, @descripcion varchar(500))
AS 
DECLARE
	@descripcionAntigua varchar(500)
BEGIN	
	set @descripcionAntigua= (select descripcion from reporte where id= @idReporte);
	DELETE FROM reporte_informacion WHERE idReporte= @idReporte;
	update reporte set descripcion= @descripcionAntigua + ' '+ @descripcion where id= @idReporte;
	update reporte set estadoReporte= 'nuevo' where id= @idReporte;
END;

go

CREATE PROCEDURE solicitudMasInformacionReporte( 
									@idReporteVar int,									
									@informacionVar VARCHAR(500)
								)
as
DECLARE
	@ultimoRegistro INT
BEGIN
	INSERT INTO reporte_informacion(idReporte,observacion)
		VALUES(@idReporteVar,@informacionVar);
END;


go
-------------------------------Nuevos Gabiel------------------------------

CREATE PROCEDURE obtenerPCsLaboratorio(
								@idReporte VARCHAR(100)
								)
AS 
BEGIN		
			 --SELECT * FROM computadora WHERE establecimiento=@nombreLab;	
			 select * from detalleReporte as inf inner join computadora as comp on (comp.codigo= inf.codigoComputadora) where inf.idReporte = @idReporte;
	return
END;
go
CREATE PROCEDURE obtenerPCsLaboratorioReporte(
								@nombreLab VARCHAR(100)
								)
AS 
BEGIN		
			 --SELECT * FROM computadora WHERE establecimiento=@nombreLab;	
			 select * from computadora as c where c.establecimiento = @nombreLab;
	return
END;
go


CREATE PROCEDURE actualizarDetalleReporte(
								@idReporte int,
								@idPC  VARCHAR(100),
								@color VARCHAR(100),
								@descripcion VARCHAR(100)
								)
AS 
BEGIN	
	UPDATE detalleReporte
		SET estadoComputadora = @color,
			descripcion = @descripcion
		WHERE idReporte = @idReporte AND codigoComputadora = @idPC;
END


go

--DROP PROCEDURE crearPC
CREATE PROCEDURE crearPc(
								@idPC VARCHAR(100),
								@x  VARCHAR(100),
								@y VARCHAR(100),
								@nombreLab VARCHAR(100)
								)
AS 
BEGIN 
	IF @idPC = 'C-'
		INSERT into computadora(codigo,posicionX,posicionY,establecimiento) 
			values	(@idPC + cast(((SELECT COUNT(*) FROM computadora)+1) as varchar),@x ,@y,@nombreLab);
	ELSE
		UPDATE computadora
			SET posicionX = @x,
				posicionY = @y,
				establecimiento = @nombreLab
			WHERE codigo = @idPC;
END 





go
--EXEC crearPc 'IC-1050','100.1','122.2','asd';

CREATE PROCEDURE obtenerLabs
AS 
BEGIN		
	select establecimiento from computadora GROUP BY establecimiento;
	return
END;

--EXEC obtenerLabs
go 
CREATE PROCEDURE borrarPC(
								@idPC VARCHAR(100)
								)
AS 
BEGIN		
	delete computadora where codigo = @idPC;
	return
END;

--EXEC borrarPC "C-30"
------------------------------------------------------------- nuevo..... funciones para push
go


CREATE TABLE usuario_token(
	nombreUsuario VARCHAR(100) NOT NULL PRIMARY KEY,
	token VARCHAR(600) NOT NULL,
	CONSTRAINT FK_usuarioToken_nombreUsuario FOREIGN KEY(nombreUsuario) REFERENCES usuario ON DELETE CASCADE ON UPDATE CASCADE
)

go

CREATE PROCEDURE actualizarTokenUsuario(@nombreUsuarioVar varchar(100), @tokenVar varchar(600))
AS 
BEGIN	
	IF (NOT EXISTS(SELECT * FROM usuario_token where nombreUsuario=@nombreUsuarioVar))
		BEGIN
			INSERT INTO usuario_token VALUES(@nombreUsuarioVar,@tokenVar);
		END 
	ELSE
	BEGIN
		UPDATE usuario_token
						SET token = @tokenVar 
						WHERE nombreUsuario = @nombreUsuarioVar;
	END
END


GO

--obtiene el token de un usuario

CREATE PROCEDURE obtenerTokenUsuario(@idReporteVar int)
as
begin
	select TOP 1 nombreUsuario,token from usuariosReporte as ur inner join usuario_token as ut on (ur.idUsuario= ut.nombreUsuario);
end;

GO



--obtiene el token de un usuario tecnico segun su username
CREATE PROCEDURE OBTENER_TOKEN_TECNICO(@username VARCHAR(200))
as
begin
	select token FROM usuario_token WHERE nombreUsuario = @username;
end;

GO


CREATE PROCEDURE obtenerTokenAdministradores
AS 
BEGIN	
	SELECT usuario_token.token FROM usuario_token inner join usuario on (usuario.nombreUsuario= usuario_token.nombreUsuario and
	usuario.rol= 'Administrador')
END



go


CREATE PROCEDURE obtenerTecnicosAsignadosAReporte(
	@idReporteP int
)
AS 
BEGIN	
	SELECT u.nombreUsuario,u.contrasena,u.nombre,u.apellido1,u.apellido2,u.correo,u.correo,u.telefono,u.rol, u.activo 
		FROM usuario AS u inner join usuariosReporte AS ur ON (u.nombreUsuario = ur.idUsuario) 
		WHERE ur.idReporte = @idReporteP and ur.rol = 'Tecnico'
END

go


CREATE PROCEDURE obtenerUsuariosNuevos
as
begin
	select * from usuario where activo= 'No'
end;

GO


CREATE PROCEDURE eliminarTecnicosReporte(
	@idReporteP int
)
AS 
BEGIN	
	delete usuariosReporte where idReporte=@idReporteP and rol = 'Tecnico';
END;







--INFORMACION PRUEBA


-- *******************usuario***********************


--insertar
exec insertarUsuario 'JCP27','123','Juliana','Campos','Parajeles','JCP27@hotmail.com','86806809','Administrador','s';
exec insertarUsuario'Marcos06','Marcos06','Marcos','Elizondo','Torres','yyy@hotmail.com','78128994','Profesor','s';
exec insertarUsuario 'FabiR03','FabiR03','Fabiola','Rosales','Fonseca','fff@hotmail.com','54210012','Operador','s';
exec insertarUsuario 'Brenes01','Brenes01','Jose','Brenes','Rojas','kkk@hotmail.com','88610001','Tecnico','s';

exec insertarDetalleReporte '1';
SELECT * FROM usuario



--modificar

exec actualizarUsuario 'Brenes01','Brenes02','Brenes02','Josue','Brenes','Rojas','kkk@hotmail.com','88610001','Administrador','N';



--activarUsuario

exec activarUsuario 'Anthony';


--  *****************REPORTE *****************
select * from usuario

select * from usuariosReporte
select * from reporte

exec selectListaDeReportes 'Alvarado'



--usuarioReporte



--eliminar Reporte
exec eliminarReporte '2'


--modificar fechaYhora
exec modificarFechaYPrioridadReporte '3','Alto','23-03-2017';


--agregarDescripcion
exec agregarDescripcionReporte '3','La version de python debe ser la 8.2';



select * from reporte


-- ************ usuarios Reporte ***************

--insertar 
exec insertarUsuariosReporte '3','Brenes01','Tecnico'


--eliminar
exec eliminarUsuarioReporte '3','Brenes02';


select * from usuariosReporte 


-- ************* detalle reporte ***************


--insertar
exec insertarDetalleReporte '3'


SELECT * FROM computadora


--insetar computadoras
INSERT into computadora(codigo,posicionX,posicionY,establecimiento) 
values	('C-1',100,100,'LAB-01'),('C-2',250,100,'LAB-01'),('C-3',400,100,'LAB-01'),
		('C-4',550,100,'LAB-01'),('C-5',700,100,'LAB-01'),('C-6',850,100,'LAB-01'),
		('C-7',100,300,'LAB-01'),('C-8',250,300,'LAB-01'),('C-9',400,300,'LAB-01'),
		('C-10',550,300,'LAB-01'),('C-11',700,300,'LAB-01'),('C-12',850,300,'LAB-01'),
		('C-13',100,500,'LAB-01'),('C-14',250,500,'LAB-01'),('C-15',400,500,'LAB-01'),
		('C-16',550,500,'LAB-01'),('C-17',700,500,'LAB-01'),('C-18',850,500,'LAB-01'),
		('C-19',100,700,'LAB-01'),('C-20',250,700,'LAB-01'),('C-21',400,700,'LAB-01'),
		('C-22',550,700,'LAB-01'),('C-23',700,700,'LAB-01'),('C-24',850,700,'LAB-01'),
		
		('C-25',100,100,'LAB-02'),('C-26',250,100,'LAB-02'),('C-27',400,100,'LAB-02'),
		('C-28',550,100,'LAB-02'),('C-29',700,100,'LAB-02'),('C-30',850,100,'LAB-02'),
		('C-31',100,300,'LAB-02'),('C-32',250,300,'LAB-02'),('C-33',400,300,'LAB-02'),
		('C-34',550,300,'LAB-02'),('C-35',700,300,'LAB-02'),('C-36',850,300,'LAB-02'),
		('C-37',100,500,'LAB-02'),('C-38',250,500,'LAB-02'),('C-39',400,500,'LAB-02'),
		('C-40',550,500,'LAB-02'),('C-41',700,500,'LAB-02'),('C-42',850,500,'LAB-02'),
		('C-43',100,700,'LAB-02'),('C-44',250,700,'LAB-02'),('C-45',400,700,'LAB-02'),
		('C-46',550,700,'LAB-02'),('C-47',700,700,'LAB-02'),('C-48',850,700,'LAB-02'),

		
		('C-49',100,100,'Moviles'),('C-50',100,250,'Moviles'),
		('C-51',100,400,'Moviles'),('C-52',100,550,'Moviles'),
		('C-53',100,700,'Moviles'),('C-54',100,850,'Moviles'),
		('C-55',100,1000,'Moviles'),('C-56',100,1150,'Moviles'),
		
		('C-57',250,1150,'Moviles'),('C-58',400,1150,'Moviles'),
		('C-59',550,1150,'Moviles'),('C-60',700,1150,'Moviles'),
		
		('C-61',850,100,'Moviles'),('C-62',850,250,'Moviles'),
		('C-63',850,400,'Moviles'),('C-64',850,550,'Moviles'),
		('C-65',850,700,'Moviles'),('C-66',850,850,'Moviles'),
		('C-67',850,1000,'Moviles'),('C-68',850,1150,'Moviles'),

		
		SELECT * FROM computadora
		DELETE computadora where codigo = 'C-001'
--insertar usuarios NORMALES SIN PERMISO
select*from usuario



INSERT INTO usuario(nombreUsuario,contrasena,nombre,apellido1,apellido2,correo,telefono,rol,activo)
values('RosendaCVffdfddCV', '12V3', 'RosenVda', 'Rosales','Vargas', 'rose@gmail.com', '85649475','Operador','No')

select * from usuario

use mantenimiento
exec insertarUsuario 'Pamela', 'Pamela', 'Pamela', 'Pamea','Vargas', 'Pamela@gmail.com', '85649475','Tecnico','S';
exec insertarUsuario 'Claudia', 'Claudia', 'Carranza', 'Perez','Brenes', 'Clau@gmail.com', '85649475','Tecnico','S';
exec insertarUsuario 'Anmador', 'Anmador', 'Palmares', 'Rolcio','Vargas', 'Anmador@gmail.com', '85649475','Tecnico','S';
exec insertarUsuario 'Sara', 'Sara', 'Pamela', 'Hern�ndez','Vargas', 'Hernandez@gmail.com', '85649475','Tecnico','S';
exec insertarUsuario 'Roberthod', 'Roberthod', 'Olivares', 'Valles','Valles', 'Valles@gmail.com', '85649475','Tecnico','S';

exec insertarUsuario 'Alvarado', '1234', 'Alvo', 'Mendez','Ortega', 'Ortega@gmail.com', '85352475','Profesor','S';
exec insertarUsuario 'Brenda', '123', 'Brenda', 'Mora','Salazar', 'bmr@gmail.com', '85644778','Tecnico','S';
exec insertarUsuario 'Jenny', '312', 'Jenny', 'Quesada','Cubillo', 'jqc@gmail.com', '88649413','Administrador','S';
exec insertarUsuario 'Anthony', '132', 'Anthony', 'Montero','Campos', 'montero@gmail.com', '89619415','Profesor','S';
exec insertarUsuario 'Maria', '321', 'Maria', 'Rojas','Brenes', 'Maria@gmail.com', '85639405','Tecnico','S';






use mantenimiento
--insertar ---

select * from reporte
EXEC crearReporte 'nuevo','05-11-2017','Instalar python en computadoras del lab-02','LAB-02','Alvarado';
EXEC crearReporte 'conPrioridad','05-10-2017','Instalar python en computadoras del lab-02','LAB-02','Alvarado';
EXEC crearReporte 'conPrioridad','02-11-2017','Instalar python en computadoras del lab-02','LAB-02','Alvarado';
EXEC crearReporte 'conPrioridad','02-11-2017','Instalar python en computadoras del lab-02','LAB-02','Alvarado';						






----__________________________________________________________________________

use mantenimiento




exec solicitudMasInformacionReporte 4, 'Cu�l vers��n de Ruby'
exec solicitudMasInformacionReporte 3, 'Indicar cu�l versi�n de Python e indicar como cu�ndo se realizar� exactamente'
exec solicitudMasInformacionReporte 24, 'Indicar cu�l versi�n para poder instalar'
select * from reporte where estadoReporte= 'informacion'
select * from reporte_informacion
select * from reporte


use mantenimiento
select * 
update reporte set estadoReporte= 'informacion' where id=4


drop procedure actualizarInformacionReporte

exec actualizarInformacionReporte 3, 'es la nuemro 2'


select * from reporte where id = 7


update reporte set estadoReporte= 'informacion'
EXEC obtenerPCsLaboratorio '1';

EXEC actualizarDetalleReporte '1','C-007','Verde','listo';

DROP PROCEDURE actualizarDetalleReporte;


--					FUNCIONES NUEVAS 22/02/2018

go


CREATE PROCEDURE cambiarActivo(@idUsuarioV varchar(100))
AS 
BEGIN	
	UPDATE usuario SET activo= 'Si' WHERE @idUsuarioV = nombreUsuario
END

go

CREATE PROCEDURE loginUser(@nombreU varchar(100), @contr varchar(600))
AS 
BEGIN	
	SELECT * FROM usuario WHERE nombreUsuario = @nombreU and contrasena= @contr and activo= 'Si'
END

------------------------------------------------------------------------


