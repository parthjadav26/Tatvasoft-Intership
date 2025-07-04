CREATE TABLE customers(
	id SERIAL PRIMARY KEY,
	name VARCHAR(50),
	email VARCHAR(50) NOT NULL UNIQUE,
	created_at TIMESTAMP
);

SELECT * FROM customer;

ALTER TABLE customers ADD COLUMN created_at TIMESTAMP

ALTER TABLE customer RENAME TO customers

ALTER TABLE customers DROP COLUMN salary;

CREATE TABLE orders(
	order_id SERIAL PRIMARY KEY,
	order_date TIMESTAMP,
	order_no INTEGER,
	order_amt FLOAT,
	cust_id INTEGER REFERENCES customers(id) 
);

SELECT * FROM orders;

INSERT INTO customers (name, email,created_at) VALUES
('Alice Smith', 'alice@example.com',NOW()),
('Bob Johnson', 'bob@example.com',NOW()),
('Carol Davis', 'carol@example.com',NOW()),
('David Miller', 'david@example.com',NOW()),
('Eve Wilson', 'eve@example.com',NOW()),
('Frank Brown', 'frank@example.com',NOW()),
('Grace Lee', 'grace@example.com',NOW()),
('Hank Adams', 'hank@example.com',NOW()),
('Ivy Scott', 'ivy@example.com',NOW()),
('Jack White', 'jack@example.com',NOW());

INSERT INTO orders (order_date, order_no, order_amt, cust_id) VALUES
(NOW(), 2001, 150.00, 1),  
(NOW(), 2002, 300.00, 2), 
(NOW(), 2003, 450.00, 1), 
(NOW(), 2004, 120.00, 3), 
(NOW(), 2005, 220.00, 2),
(NOW(), 2006, 500.00, 4),
(NOW(), 2007, 330.00, 5),
(NOW(), 2008, 190.00, 3),
(NOW(), 2009, 210.00, 1),
(NOW(), 2010, 275.00, 5);


SELECT * FROM customers as c INNER JOIN orders as o ON c.id=o.cust_id;

SELECT * FROM customers as c LEFT JOIN orders as o ON c.id=o.cust_id;

SELECT * FROM customers as c RIGHT JOIN orders as o ON c.id=o.cust_id;

SELECT * FROM customers as c LEFT JOIN orders as o ON c.id=o.cust_id WHERE o.order_id IS NULL;

SELECT * FROM customers as c RIGHT JOIN orders as o ON c.id=o.cust_id WHERE c.id IS NULL;

SELECT * FROM customers as c FULL JOIN orders as o ON c.id=o.cust_id WHERE o.order_id IS NULL OR c.id IS NULL;


SELECT * FROM customers ORDER BY name ASC

SELECT * FROM customers LIMIT 5;


SELECT DISTINCT(cust_id) FROM orders;

SELECT order_id,SUM(order_amt) AS TOTAL FROM orders GROUP BY order_id;

SELECT cust_id,SUM(order_amt) AS TOTAL,COUNT(order_no) FROM orders GROUP BY cust_id HAVING COUNT(order_no)>1;





SELECT name FROM customers WHERE id = ANY (SELECT cust_id FROM orders WHERE order_amt > 400 );

SELECT name FROM customers WHERE id IN ( SELECT cust_id FROM orders GROUP BY cust_id HAVING MIN(order_amt) > 200);

SELECT * FROM orders WHERE order_amt > ALL (SELECT order_amt FROM orders WHERE cust_id = 2);

SELECT name FROM customers c WHERE EXISTS (SELECT 1 FROM orders o WHERE o.cust_id = c.id);












































































