# Edison Engineering — API Reference for Frontend (Angular)

Purpose: a concise reference the frontend team can use to call the Web API.

**Base URL**
- Development: `https://<api-host>`
- All endpoints are versioned and use the URL segment versioning: `/api/v1/...`

**Common response wrapper**
- Successful and error responses use the `ApiResponse<T>` envelope:
  - `{ Success: bool, Message: string, Data?: T, Errors?: string[] }`

**Authentication**
- JWT Bearer on protected endpoints.
- Login endpoint: `POST /api/v1/auth/login` — returns `LoginResponseDto` with `Token`, `RefreshToken`, `Expiration`, `Name`, `Email`, `Role`.
- Include header: `Authorization: Bearer <token>`

Example (cURL):
```bash
curl -X POST https://api.example.com/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Secret123"}'
```

Angular HttpClient login example:
```ts
// auth.service.ts
login(creds: {email: string; password: string}) {
  return this.http.post<ApiResponse<LoginResponseDto>>(
    `${this.baseUrl}/api/v1/auth/login`,
    creds
  );
}
```

**Rate limiting**
- Global: 100 requests per minute per client IP (with small queue).
- Login endpoint: stricter policy — 5 requests per minute per IP (no queue). Handle HTTP 429 responses.
- When blocked the API returns `429` and body:
```json
{ "success": false, "message": "Too many requests. Please try again later." }
```

**Output caching (server-side)**
- Server uses output cache policies. Examples:
  - `blogs-cache` — cached for 5 minutes
  - `menu-cache` — cached for 30 minutes
  - `services-cache` — cached for 10 minutes
  - `cities-cache` — cached for 10 minutes
- Treat cache as server-controlled; POST/PUT/DELETE will return updated data when completed.

**Health endpoints**
- Liveness: `GET /health/live` — basic liveness check.
- Readiness: `GET /health/ready` — returns JSON with overall status and per-check details. Example response:
```json
{
  "Status": "Healthy",
  "Checks": [
    { "Name": "Database", "Status": "Healthy", "Duration": "00:00:00.1234567" }
  ],
  "TotalDuration": "00:00:00.1234567"
}
```
- General `GET /health` also exists.

---
**Endpoints (summary & examples)**

1) Auth
- POST `/api/v1/auth/login`
  - Body: `LoginRequestDto` `{ email, password }`
  - Success: 200 `{ Success:true, Data: { Token, RefreshToken, Expiration, ... }}`
- POST `/api/v1/auth/refresh`
  - Body: `{ refreshToken: string, token: string }` (see DTO in code)

2) Blogs
- GET `/api/v1/blogs`
  - Query: `page` (int, default 1), `pageSize` (int, default 10), `search` (string)
  - Cached by `blogs-cache` (5 minutes)
  - Returns `ApiResponse<PagedResponse<BlogListDto>>` where `BlogListDto` contains `{ Title, Slug, ImageUrl?, CreatedAt }`.
  - Example: `GET /api/v1/blogs?page=1&pageSize=10&search=solar`

- GET `/api/v1/blogs/{slug}`
  - Returns `ApiResponse<BlogDto>` where `BlogDto` has `{ Title, Slug, Content, MetaTitle?, MetaDescription?, ImageUrl?, CreatedAt }`.

- POST `/api/v1/blogs` (admin)
  - Requires permission `blog.create` (authenticated admin JWT)
  - Body: `CreateBlogDto` — title, slug, content, etc.

- PUT `/api/v1/blogs/{id}` (admin)
  - Requires `blog.edit` permission

- DELETE `/api/v1/blogs/{id}` (admin)
  - Requires `blog.delete` permission

3) Menu
- GET `/api/v1/menu`
  - Returns menu tree: `ApiResponse<IEnumerable<MenuDto>>`
  - Cached by `menu-cache` (30 minutes)

4) Services
- GET `/api/v1/services`
  - Returns list of service categories: `ApiResponse<IEnumerable<ServiceCategoryDto>>`
  - Cached by `services-cache` (10 minutes)
- GET `/api/v1/services/{categorySlug}`
  - Returns `ServiceCategoryDto`
- GET `/api/v1/services/{categorySlug}/{serviceSlug}`
  - Returns `ServiceDto`

5) Cities
- GET `/api/v1/cities`
  - Returns `ApiResponse<IEnumerable<CityDto>>` (cached by `cities-cache`)
- GET `/api/v1/cities/{slug}`
  - Returns `CityDto`
- GET `/api/v1/cities/{slug}/projects`
  - Returns `IEnumerable<ProjectDto>` for the city

6) Leads (forms / contact)
- POST `/api/v1/leads`
  - Body: `CreateLeadDto`
    - Required fields: `Name`, `Phone`, `City`, `ServiceType`
    - Optional: `Email`, `Message`, `Source`
  - Returns: `201 Created` with `ApiResponse<string>` on success

Angular example to submit a lead:
```ts
const payload = {
  name: 'Jane Doe',
  phone: '+911234567890',
  email: 'jane@example.com',
  city: 'Pune',
  serviceType: 'Home Solar',
  message: 'Interested in a quote',
  source: 'website'
};

this.http.post<ApiResponse<string>>(`${this.baseUrl}/api/v1/leads`, payload)
  .subscribe(res => {
    if (res.success) { /* show thanks UI */ }
  });
```

7) Uploads (resume)
- POST `/api/uploads/resume` (Requires `Authorization` header)
  - Content-Type: `multipart/form-data`
  - Form field: `file` (IFormFile)
  - Allowed extensions: `.pdf`, `.doc`, `.docx` — limit 5 MB
  - Returns `ApiResponse<FileUploadResponseDto>` where `FileUrl` is the saved file URL

cURL file upload example:
```bash
curl -X POST https://api.example.com/api/uploads/resume \
  -H "Authorization: Bearer <token>" \
  -F "file=@/path/to/resume.pdf"
```

Angular upload (HttpClient):
```ts
const fd = new FormData();
fd.append('file', file, file.name);
this.http.post<ApiResponse<FileUploadResponseDto>>(`${this.baseUrl}/api/uploads/resume`, fd, {
  headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` })
});
```

**Validation & error handling**
- Validation errors return a `400 Bad Request` with `ApiResponse<object>` where `Errors` is a string array.
- Authentication failures return `401` with an `ApiResponse<string>`.
- Not found endpoints return `404` with `ApiResponse<string>`.

**Headers the frontend should set**
- `Content-Type: application/json` for JSON requests (except file uploads)
- `Authorization: Bearer <token>` for protected routes
- Optionally accept `application/json`

**Developer tips**
- Respect rate-limiting — implement exponential backoff for 429 responses.
- Treat cached endpoints as eventually consistent; if you need latest data, call non-GET or implement cache-busting.
- Use the `RefreshToken` flow to refresh JWTs when `Expiration` is near.

**Useful server files & DTOs**
- `Program.cs` — startup, rate limiting, caching, health checks
- `Controllers/` — controller routes and behavior
- DTOs in `EdisonEngineering.Application/DTOs/` (e.g., `CreateLeadDto`, `BlogQueryDto`, `BlogDto`, `LoginRequestDto`, `LoginResponseDto`)

---
If you want, I can:
- Generate Swagger/OpenAPI JSON for direct import into Postman (done via swagger UI already), or
- Produce a one-page Postman collection or an OpenAPI spec file exported from the running app.

Requested next step? (export Postman collection / generate OpenAPI file / other)
