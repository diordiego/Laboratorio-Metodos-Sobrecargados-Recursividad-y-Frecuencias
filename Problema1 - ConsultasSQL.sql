-- ============================================================
-- PROBLEMA #1 - Consultas SQL (3) : Inyección SQL
-- Laboratorio HPA - OWASP A05: Inyección
-- ============================================================
-- Objetivo: reproducir las 3 consultas de la profesora sobre una
-- tabla de prueba y demostrar POR QUE una consulta construida con
-- concatenacion de cadenas es vulnerable a inyeccion SQL.
-- ============================================================

-- ------------------------------------------------------------
-- 0) Preparacion: base de datos y tabla de prueba
-- ------------------------------------------------------------
CREATE DATABASE IF NOT EXISTS lab_hpa;
USE lab_hpa;

DROP TABLE IF EXISTS permisos;
CREATE TABLE permisos (
    id                 INT AUTO_INCREMENT PRIMARY KEY,
    Usuario            VARCHAR(50)  NOT NULL,
    Contrasena         VARCHAR(100) NOT NULL,
    Administrador      TINYINT(1)   DEFAULT 0,
    Correo             VARCHAR(100),
    UsuarioSistema     TINYINT(1)   DEFAULT 0,
    FechaModificacion  DATE,
    Modulo_Personal    TINYINT(1)   DEFAULT 0,
    Modulo_Horario     TINYINT(1)   DEFAULT 0,
    Modulo_Vacaciones  TINYINT(1)   DEFAULT 0
);

-- Datos de prueba (contrasenas de ejemplo, no reales)
INSERT INTO permisos
    (Usuario, Contrasena, Administrador, Correo, UsuarioSistema, FechaModificacion,
     Modulo_Personal, Modulo_Horario, Modulo_Vacaciones)
VALUES
    ('vielkarosmery', 'clave001', 1, 'vreyes@correo.com',   0, '2020-01-10', 1, 1, 0),
    ('armando',       'clave002', 1, 'amartinez@correo.com',0, '2020-02-15', 1, 1, 1),
    ('pbarbax',       'clave003', 1, 'pbarba@correo.com',   1, '2020-03-20', 0, 0, 0),
    ('root',          'clave004', 1, 'admin@correo.com',    0, '2020-05-20', 1, 1, 1);


-- ------------------------------------------------------------
-- CASO 1 : Consulta "normal" (la que el programador pretendia)
-- ------------------------------------------------------------
-- Asi se veria un login legitimo: usuario y contrasena correctos.
-- Solo deberia devolver 1 fila si las credenciales son validas.
SELECT * FROM permisos
WHERE Usuario = 'armando' AND Contrasena = 'clave002';


-- ------------------------------------------------------------
-- CASO 2 : Inyeccion con OR '1'='1' (autenticacion siempre verdadera)
-- ------------------------------------------------------------
-- Si el sistema arma la cadena concatenando lo que el usuario escribe:
--   "...WHERE Usuario = '" + usuario + "' AND Contrasena = '" + pass + "'"
-- y el atacante escribe en los campos del formulario:
--   Usuario:    ' or '1'='1
--   Contrasena: ' OR '1'='1
-- la consulta resultante queda asi:
SELECT * FROM permisos
WHERE Usuario = '' OR '1'='1' AND Contrasena = '' OR '1'='1';
-- RESULTADO: como '1'='1' siempre es verdadero y se usa OR, la condicion
-- completa es verdadera para TODAS las filas -> devuelve la tabla entera.
-- El atacante entra sin saber ninguna contrasena.


-- ------------------------------------------------------------
-- CASO 3 : Inyeccion con comentario (--) para anular la contrasena
-- ------------------------------------------------------------
-- El atacante escribe en el campo Usuario:   root'; --
-- La consulta queda:
SELECT * FROM permisos
WHERE Usuario = 'root'; -- ' AND Contrasena = 'mypassword';
-- RESULTADO:
--   '   cierra antes de tiempo el texto esperado
--   ;   indica fin de la instruccion
--   --  comenta (ignora) todo lo que sigue, incluyendo la validacion
--       de la contrasena.
-- El sistema ejecuta solo: SELECT * FROM permisos WHERE Usuario='root'
-- y entrega al usuario 'root' sin pedir contrasena.


-- ============================================================
-- CONCLUSION:
-- Las 3 consultas muestran que concatenar la entrada del usuario
-- directamente en el SQL permite alterar la LOGICA de la consulta.
-- La solucion (Problema #2) son las consultas parametrizadas, donde
-- la entrada se trata siempre como DATO y nunca como codigo.
-- ============================================================