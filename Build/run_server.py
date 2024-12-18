#!/usr/bin/python
"""
runServer.py

A simple HTTP server for serving Unity WebGL builds with Gzip support.

Usage:
    python runServer.py

Features:
    - Serves files from the current directory.
    - Adds 'Content-Encoding: gzip' header for .gz files (Unity WebGL support).
    - Default port: 8013.

Requirements:
    - Python 3.x

How to Access:
    - Open a web browser and navigate to: http://localhost:8013
"""

import http.server
import socketserver

# Set the port to serve the HTTP server on
PORT = 8013


class GzipHTTPRequestHandler(http.server.SimpleHTTPRequestHandler):
    """
    Unity game testing locally
    """
    def end_headers(self):
        # Check if the file has a .gz extension and set the Content-Encoding
        # header
        if self.path.endswith(".gz"):
            self.send_header("Content-Encoding", "gzip")
        # Ensure the correct Content-Type for JS, data files, etc.
        if self.path.endswith(".js.gz"):
            self.send_header("Content-Type", "application/javascript")
        elif self.path.endswith(".data.gz"):
            self.send_header("Content-Type", "application/octet-stream")
        elif self.path.endswith(".wasm.gz"):
            self.send_header("Content-Type", "application/wasm")
        super().end_headers()


# Set up and start the HTTP server
with socketserver.TCPServer(("", PORT), GzipHTTPRequestHandler) as httpd:
    print(f"Serving at port {PORT}, with Gzip headers for .gz files")
    print(f"Access the server at http://localhost:{PORT}")
    httpd.serve_forever()
