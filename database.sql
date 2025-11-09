-- ====================================
-- Library Management System Database
-- ====================================
-- PostgreSQL Database Schema
-- Version: 1.0
-- Created: 2024
-- ====================================

-- Drop existing tables if they exist
DROP TABLE IF EXISTS verification_codes CASCADE;
DROP TABLE IF EXISTS book_events CASCADE;
DROP TABLE IF EXISTS books CASCADE;
DROP TABLE IF EXISTS authors CASCADE;
DROP TABLE IF EXISTS categories CASCADE;
DROP TABLE IF EXISTS branches CASCADE;
DROP TABLE IF EXISTS accounts CASCADE;
DROP TABLE IF EXISTS roles CASCADE;
DROP TABLE IF EXISTS event_types CASCADE;

-- ====================================
-- REFERENCE TABLES
-- ====================================

-- Roles table
CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Event types table (Request, Return, Taken, Saved)
CREATE TABLE event_types (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ====================================
-- CORE TABLES
-- ====================================

-- Authors table
CREATE TABLE authors (
    id SERIAL PRIMARY KEY,
    full_name VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Categories table
CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Branches (Library branches) table
CREATE TABLE branches (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL UNIQUE,
    address TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Accounts (Users) table
CREATE TABLE accounts (
    id SERIAL PRIMARY KEY,
    login VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    full_name VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(255),
    role_id INTEGER NOT NULL REFERENCES roles(id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Verification codes table (for email verification during registration)
CREATE TABLE verification_codes (
    id SERIAL PRIMARY KEY,
    email VARCHAR(255) NOT NULL,
    code VARCHAR(6) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    expiry_date TIMESTAMP NOT NULL,
    is_used BOOLEAN DEFAULT FALSE,
    account_id INTEGER REFERENCES accounts(id) ON DELETE CASCADE,
    CONSTRAINT unique_email_code UNIQUE (email, code)
);

-- Books table
CREATE TABLE books (
    id SERIAL PRIMARY KEY,
    title VARCHAR(500) NOT NULL,
    description TEXT,
    fragment TEXT,
    cover_link VARCHAR(500),
    author_id INTEGER NOT NULL REFERENCES authors(id) ON DELETE CASCADE,
    category_id INTEGER NOT NULL REFERENCES categories(id) ON DELETE CASCADE,
    branch_id INTEGER NOT NULL REFERENCES branches(id) ON DELETE CASCADE,
    publish_year INTEGER,
    isbn VARCHAR(20),
    quantity INTEGER DEFAULT 1,
    available_quantity INTEGER DEFAULT 1,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Book Events table (Borrowing, returns, reservations)
CREATE TABLE book_events (
    id SERIAL PRIMARY KEY,
    account_id INTEGER NOT NULL REFERENCES accounts(id) ON DELETE CASCADE,
    book_id INTEGER NOT NULL REFERENCES books(id) ON DELETE CASCADE,
    event_type_id INTEGER NOT NULL REFERENCES event_types(id),
    event_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    due_date TIMESTAMP,
    return_date TIMESTAMP,
    librarian_id INTEGER REFERENCES accounts(id),
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ====================================
-- INDEXES FOR PERFORMANCE
-- ====================================

CREATE INDEX idx_books_title ON books(title);
CREATE INDEX idx_books_author ON books(author_id);
CREATE INDEX idx_books_category ON books(category_id);
CREATE INDEX idx_books_branch ON books(branch_id);
CREATE INDEX idx_book_events_account ON book_events(account_id);
CREATE INDEX idx_book_events_book ON book_events(book_id);
CREATE INDEX idx_book_events_date ON book_events(event_date);
CREATE INDEX idx_accounts_login ON accounts(login);
CREATE INDEX idx_accounts_email ON accounts(email);
CREATE INDEX idx_verification_codes_email ON verification_codes(email);
CREATE INDEX idx_verification_codes_expiry ON verification_codes(expiry_date);

-- ====================================
-- SAMPLE DATA
-- ====================================

-- Insert roles
INSERT INTO roles (name) VALUES 
    ('Admin'),
    ('Librarian'),
    ('User');

-- Insert event types
INSERT INTO event_types (name, description) VALUES 
    ('Request', 'Book request by user'),
    ('Return', 'Book returned by user'),
    ('Taken', 'Book taken/borrowed by user'),
    ('Saved', 'Book saved to wishlist');

-- Insert sample authors
INSERT INTO authors (full_name) VALUES 
    ('Александр Пушкин'),
    ('Лев Толстой'),
    ('Федор Достоевский'),
    ('Антон Чехов'),
    ('Иван Тургенев'),
    ('Михаил Булгаков'),
    ('Николай Гоголь');

-- Insert sample categories
INSERT INTO categories (name) VALUES 
    ('Классическая литература'),
    ('Романы'),
    ('Драматургия'),
    ('Поэзия'),
    ('Фантастика'),
    ('Детективы'),
    ('Научная литература');

-- Insert sample branches
INSERT INTO branches (name, address) VALUES 
    ('Центральная библиотека', 'ул. Ленина, 1'),
    ('Филиал №1', 'ул. Пушкина, 10'),
    ('Детская библиотека', 'ул. Гагарина, 5'),
    ('Научная библиотека', 'ул. Ломоносова, 15');

-- Insert sample admin account (password: admin123)
-- Note: In production, password should be hashed using BCrypt
INSERT INTO accounts (login, password, full_name, phone, email, role_id) VALUES 
    ('admin', '$2a$11$hashed_password_here', 'Администратор', '+79991234567', 'admin@library.com', 1),
    ('librarian', '$2a$11$hashed_password_here', 'Библиотекарь Иванов', '+79991234568', 'librarian@library.com', 2),
    ('user', '$2a$11$hashed_password_here', 'Читатель Петров', '+79991234569', 'user@library.com', 3);

-- Insert sample books
INSERT INTO books (title, description, fragment, cover_link, author_id, category_id, branch_id, publish_year, isbn, quantity, available_quantity) VALUES 
    ('Евгений Онегин', 'Роман в стихах', 'Мой дядя самых честных правил...', '/covers/onegin.jpg', 1, 4, 1, 1833, '978-5-17-123456-1', 5, 5),
    ('Война и мир', 'Роман-эпопея', 'Ну, князь, Генуя и Лукка...', '/covers/war_peace.jpg', 2, 2, 1, 1869, '978-5-17-123456-2', 3, 2),
    ('Преступление и наказание', 'Роман', 'В начале июля, в чрезвычайно жаркое время...', '/covers/crime.jpg', 3, 2, 2, 1866, '978-5-17-123456-3', 4, 4),
    ('Вишневый сад', 'Пьеса', 'Действие первое...', '/covers/cherry.jpg', 4, 3, 1, 1904, '978-5-17-123456-4', 2, 2),
    ('Отцы и дети', 'Роман', 'Ну, Петр? Не видать еще?', '/covers/fathers.jpg', 5, 2, 3, 1862, '978-5-17-123456-5', 3, 3),
    ('Мастер и Маргарита', 'Роман', 'В час жаркого весеннего заката...', '/covers/master.jpg', 6, 5, 1, 1967, '978-5-17-123456-6', 4, 3);

-- Insert sample book events
INSERT INTO book_events (account_id, book_id, event_type_id, event_date, due_date, librarian_id) VALUES 
    (3, 1, 1, NOW() - INTERVAL '5 days', NOW() + INTERVAL '9 days', 2),
    (3, 2, 3, NOW() - INTERVAL '3 days', NOW() + INTERVAL '11 days', 2),
    (3, 4, 4, NOW() - INTERVAL '2 days', NULL, NULL);

-- ====================================
-- VIEWS FOR COMMON QUERIES
-- ====================================

-- View: Book list with all related information
CREATE OR REPLACE VIEW v_books_full AS
SELECT 
    b.id,
    b.title,
    b.description,
    b.fragment,
    b.cover_link,
    b.publish_year,
    b.isbn,
    b.quantity,
    b.available_quantity,
    a.id as author_id,
    a.full_name as author_name,
    c.id as category_id,
    c.name as category_name,
    br.id as branch_id,
    br.name as branch_name,
    b.created_at,
    b.updated_at
FROM books b
INNER JOIN authors a ON b.author_id = a.id
INNER JOIN categories c ON b.category_id = c.id
INNER JOIN branches br ON b.branch_id = br.id;

-- View: Active book requests
CREATE OR REPLACE VIEW v_active_book_requests AS
SELECT 
    be.id as event_id,
    be.account_id,
    acc.full_name as user_name,
    be.book_id,
    b.title as book_title,
    a.full_name as author_name,
    c.name as category_name,
    br.name as branch_name,
    et.name as event_type_name,
    be.event_date,
    be.due_date
FROM book_events be
INNER JOIN accounts acc ON be.account_id = acc.id
INNER JOIN books b ON be.book_id = b.id
INNER JOIN authors a ON b.author_id = a.id
INNER JOIN categories c ON b.category_id = c.id
INNER JOIN branches br ON b.branch_id = br.id
INNER JOIN event_types et ON be.event_type_id = et.id
WHERE be.return_date IS NULL;

-- ====================================
-- FUNCTIONS
-- ====================================

-- Function to update book availability when event is created
CREATE OR REPLACE FUNCTION update_book_availability()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.event_type_id = 3 THEN -- Taken
        UPDATE books 
        SET available_quantity = available_quantity - 1 
        WHERE id = NEW.book_id AND available_quantity > 0;
    ELSIF NEW.event_type_id = 2 THEN -- Return
        UPDATE books 
        SET available_quantity = available_quantity + 1 
        WHERE id = NEW.book_id;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Create trigger
CREATE TRIGGER trg_update_book_availability
AFTER INSERT ON book_events
FOR EACH ROW
EXECUTE FUNCTION update_book_availability();

-- ====================================
-- PERMISSIONS (Adjust as needed)
-- ====================================

-- GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO your_app_user;
-- GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO your_app_user;

-- ====================================
-- END OF SCRIPT
-- ====================================
