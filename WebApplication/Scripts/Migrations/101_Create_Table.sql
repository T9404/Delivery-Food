CREATE TABLE IF NOT EXISTS dishes (
    id UUID PRIMARY KEY,
    name VARCHAR(255),
    description TEXT,
    price INTEGER,
    image VARCHAR(255),
    vegetarian BOOLEAN,
    rating DOUBLE PRECISION,
    category VARCHAR(255),
    count_ratings INTEGER
);