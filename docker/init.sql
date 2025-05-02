CREATE TABLE IF NOT EXISTS telemetry (
                                         id SERIAL PRIMARY KEY,
                                         device_id TEXT NOT NULL,
                                         timestamp TIMESTAMP NOT NULL,
                                         temperature REAL NOT NULL,
                                         humidity REAL NOT NULL
);
