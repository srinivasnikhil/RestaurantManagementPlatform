from fastapi import FastAPI

app = FastAPI(title="Restaurant AI Service")

@app.get("/health")
def health():
    return {"status": "healthy", "service": "ai"}