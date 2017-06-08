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
	posicion INT not null,
	establecimiento VARCHAR(100) not null,
	CONSTRAINT CK_computadora_establecimiento CHECK(establecimiento in ('LAB-01','LAB-02','Miniauditorio','Moviles','SIRZEE')),
);




CREATE TABLE reporte_informacion(
	idReporte INT PRIMARY KEY,	
	observacion VARCHAR(500) NOT NULL
	CONSTRAINT FK_idreporte_reporte_informacion FOREIGN KEY(idReporte) REFERENCES reporte ON DELETE CASCADE ON UPDATE CASCADE	
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
	CONSTRAINT PK_detalleReporte_idReporte_codigoComputadora PRIMARY KEY(idReporte,codigoComputadora),
	CONSTRAINT FK_detalleReporte_idReporte FOREIGN KEY(idReporte)REFERENCES reporte ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_detalleReporte_codigoComputadora FOREIGN KEY(codigoComputadora)REFERENCES computadora ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT CK_detalleReporte_estadoComputadora CHECK (estadoComputadora in ('Rojo','Amarillo','Verde'))
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

go

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

--FUNCIONES DETALLE REPORTE

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
	order by c.posicion;
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




--lista de reportes de un usuario
CREATE PROCEDURE selectListaDeReportes(@personaVar varchar(100))
AS 
BEGIN		

	select reporte.id,reporte.estadoReporte, reporte.prioridadReporte, reporte.fechaReporte,
	reporte.fechaFinalizacion, reporte.descripcion, reporte.establecimiento,  
	usuario.nombreUsuario, (usuario.nombre + ' ' + usuario.apellido1 + ' ' +  usuario.apellido2) as nombre		
	
	 from usuario inner join usuariosReporte on(usuariosReporte.idUsuario = usuario.nombreUsuario)
	 inner join reporte on (reporte.id= usuariosReporte.idReporte and usuariosReporte.idUsuario=  'Alvarado') 
END;



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



--devuelve los usuarios de un rol en específico o a todos los usuarios
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

drop procedure listaDeUsuario
exec listaDeUsuario 'Profesor'

GO





--INFORMACION PRUEBA


-- *******************usuario***********************


--insertar
exec insertarUsuarioAdministrador 'JCP27','123','Juliana','Campos','Parajeles','JCP27@hotmail.com','86806809','Administrador';
exec insertarUsuarioAdministrador 'Marcos06','Marcos06','Marcos','Elizondo','Torres','yyy@hotmail.com','78128994','Profesor';
exec insertarUsuarioAdministrador 'FabiR03','FabiR03','Fabiola','Rosales','Fonseca','fff@hotmail.com','54210012','Operador';
exec insertarUsuarioAdministrador 'Brenes01','Brenes01','Jose','Brenes','Rojas','kkk@hotmail.com','88610001','Tecnico';


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


SELECT * FROM detalleReporte


--insetar computadoras
INSERT into computadora(codigo,posicion,establecimiento) 
values	('C-001',1,'LAB-01'),('C-002',2,'LAB-01'),('C-003',3,'LAB-01'),
		('C-004',4,'LAB-01'),('C-005',5,'LAB-01'),('C-006',6,'LAB-01'),

		('C-007',1,'LAB-02'),('C-008',2,'LAB-02'),('C-009',3,'LAB-02'),
		('C-010',4,'LAB-02'),('C-011',5,'LAB-02'),('C-012',6,'LAB-02'),

		('C-013',1,'Miniauditorio'),('C-014',2,'Miniauditorio'),('C-015',3,'Miniauditorio'),
		('C-016',4,'Miniauditorio'),('C-017',5,'Miniauditorio'),('C-018',6,'Miniauditorio'),		

		('C-019',1,'Moviles'),('C-020',2,'Moviles'),('C-021',3,'Moviles'),
		('C-022',4,'Moviles'),('C-023',5,'Moviles'),('C-024',6,'Moviles'),

		('C-025',1,'SIRZEE'),('C-027',2,'SIRZEE'),('C-029',3,'SIRZEE'),
		('C-026',4,'SIRZEE'),('C-028',5,'SIRZEE'),('C-030',6,'SIRZEE');



--insertar usuarios NORMALES SIN PERMISO
select*from usuario



INSERT INTO usuario(nombreUsuario,contrasena,nombre,apellido1,apellido2,correo,telefono,rol,activo)
values('RosendaCVffdfddCV', '12V3', 'RosenVda', 'Rosales','Vargas', 'rose@gmail.com', '85649475','Operador','No')



use mantenimiento
exec insertarUsuario 'RosendaCVddCV', '12V3', 'RosenVda', 'Rosales','Vargas', 'rose@gmail.com', '85649475','Operador','N';
exec insertarUsuario 'Alvarado', '1234', 'Alvo', 'Mendez','Ortega', 'Ortega@gmail.com', '85352475','Profesor','S';
exec insertarUsuario 'Brenda', '123', 'Brenda', 'Mora','Salazar', 'bmr@gmail.com', '85644778','Tecnico','S';
exec insertarUsuario 'Jenny', '312', 'Jenny', 'Quesada','Cubillo', 'jqc@gmail.com', '88649413','Administrador','S';
exec insertarUsuario 'Anthony', '132', 'Anthony', 'Montero','Campos', 'montero@gmail.com', '89619415','Profesor','S';
exec insertarUsuario 'Maria', '321', 'Maria', 'Rojas','Brenes', 'Maria@gmail.com', '85639405','Tecnico','S';






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