const API_URL = '/api';
let currentUser = JSON.parse(localStorage.getItem('user'));

function getAuthHeaders() {
    const headers = { 'Content-Type': 'application/json' };
    const user = JSON.parse(localStorage.getItem('user'));
    if (user && user.token) {
        headers['Authorization'] = `Bearer ${user.token}`;
    }
    return headers;
}

async function showErrorAlert(response, defaultMsg) {
    let msg = defaultMsg;
    if (response && response.text) {
        try {
            const text = await response.text();
            try {
                const json = JSON.parse(text);
                msg = json.message || json.error || text;
            } catch {
                msg = text || defaultMsg;
            }
        } catch { }
    } else if (typeof response === 'string') {
        msg = response;
    }
    
    if (response && (response.status === 401 || response.status === 403)) {
        msg = response.status === 401 ? "Session expired. Please login again." : "Access denied: You do not have permission for this action.";
        if (response.status === 401) {
            logout();
        }
    }
    
    alert('ERROR: ' + msg + '\n\nAction required: Please review your entered values and try again.');
}

document.addEventListener('DOMContentLoaded', () => {
    updateNav();
    if (!currentUser) {
        showLogin();
    } else {
        showDashboard();
    }
});

function updateNav() {
    const navLinks = document.getElementById('navLinks');
    if (currentUser) {
        const roleId = parseInt(currentUser.roleId);
        let links = `
            <li class="nav-item"><a class="nav-link" href="#" onclick="showDashboard()">Dashboard</a></li>
            <li class="nav-item"><a class="nav-link" href="#" onclick="showProducts()">Products</a></li>
            <li class="nav-item"><a class="nav-link" href="#" onclick="showInventory()">Inventory</a></li>
        `;

        // Admin (1) or Sales Manager (4)
        if (roleId === 1 || roleId === 4) {
            links += `<li class="nav-item"><a class="nav-link" href="#" onclick="showSales()">Sales History</a></li>`;
        }

        // Admin (1) or Warehouse Manager (3)
        if (roleId === 1 || roleId === 3) {
            links += `<li class="nav-item"><a class="nav-link" href="#" onclick="showWarehouseLogs()">Warehouse Logs</a></li>`;
        }

        // Admin (1) or Shelf Manager (2)
        if (roleId === 1 || roleId === 2) {
            links += `<li class="nav-item"><a class="nav-link" href="#" onclick="showShelfLogs()">Shelf Logs</a></li>`;
        }

        links += `<li class="nav-item"><a class="nav-link" href="#" onclick="logout()">Logout (${currentUser.userName})</a></li>`;
        navLinks.innerHTML = links;
    } else {
        navLinks.innerHTML = `
            <li class="nav-item"><a class="nav-link" href="#" onclick="showLogin()">Login</a></li>
            <li class="nav-item"><a class="nav-link" href="#" onclick="showRegister()">Register</a></li>
        `;
    }
}

function logout() {
    localStorage.removeItem('user');
    currentUser = null;
    updateNav();
    showLogin();
}

function showLogin() {
    const content = document.getElementById('content');
    content.innerHTML = `
        <div class="row justify-content-center">
            <div class="col-md-4">
                <div class="card shadow">
                    <div class="card-body">
                        <h3 class="card-title text-center">Login</h3>
                        <form id="loginForm">
                            <div class="mb-3">
                                <label class="form-label">Username</label>
                                <input type="text" id="username" class="form-control" required>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Password</label>
                                <input type="password" id="password" class="form-control" required>
                            </div>
                            <button type="submit" class="btn btn-primary w-100">Login</button>
                        </form>
                        <div class="text-center mt-3">
                            <p>Don't have an account? <a href="#" onclick="showRegister()">Register here</a></p>
                            <p><a href="#" onclick="showForgotPassword()">Forgot Password?</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.getElementById('loginForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const userName = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        try {
            const response = await fetch(`${API_URL}/auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ userName, password })
            });

            if (response.ok) {
                const user = await response.json();
                localStorage.setItem('user', JSON.stringify(user));
                currentUser = user;
                updateNav();
                showDashboard();
            } else {
                if (response.status === 400) {
                    const errorJson = await response.json();
                    if (errorJson.message === "EMAIL_NOT_CONFIRMED") {
                        showConfirmEmail();
                        return;
                    }
                }
                await showErrorAlert(response, 'Login failed');
            }
        } catch (err) {
            console.error(err);
            alert('Error connecting to server.');
        }
    });
}

function showRegister() {
    const content = document.getElementById('content');
    content.innerHTML = `
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card shadow">
                    <div class="card-body">
                        <h3 class="card-title text-center">Register</h3>
                        <form id="registerForm">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">First Name</label>
                                    <input type="text" id="regFirst" class="form-control" required>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Last Name</label>
                                    <input type="text" id="regLast" class="form-control" required>
                                </div>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Username</label>
                                <input type="text" id="regUser" class="form-control" required>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Email</label>
                                <input type="email" id="regEmail" class="form-control" required>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Password</label>
                                <input type="password" id="regPass" class="form-control" required>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Role</label>
                                <select id="regRole" class="form-select">
                                    <option value="1">Admin</option>
                                    <option value="2">Shelf Manager</option>
                                    <option value="3">Warehouse Manager</option>
                                    <option value="4">Sales Manager</option>
                                </select>
                            </div>
                            <button type="submit" class="btn btn-success w-100">Register</button>
                        </form>
                        <div class="text-center mt-3">
                            <p>Already have an account? <a href="#" onclick="showLogin()">Login here</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.getElementById('registerForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const dto = {
            firstName: document.getElementById('regFirst').value,
            lastName: document.getElementById('regLast').value,
            userName: document.getElementById('regUser').value,
            email: document.getElementById('regEmail').value,
            password: document.getElementById('regPass').value,
            roleId: parseInt(document.getElementById('regRole').value)
        };

        try {
            const response = await fetch(`${API_URL}/auth/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dto)
            });

            if (response.ok) {
                alert('Registration successful! Please check your email.');
                showConfirmEmail();
            } else {
                await showErrorAlert(response, 'Registration failed');
            }
        } catch (err) {
            alert('Error connecting to server.');
        }
    });
}

async function showProducts() {
    const content = document.getElementById('content');
    content.innerHTML = `<h2>Products</h2><p>Loading products...</p>`;
    
    try {
        const response = await fetch(`${API_URL}/products`, { headers: getAuthHeaders() });
        if (!response.ok) {
            await showErrorAlert(response, 'Error loading products');
            return;
        }
        const products = await response.json();
        
        let html = `
            <div class="d-flex justify-content-between mb-3">
                <h2>Products</h2>
                <button class="btn btn-primary" onclick="showAddProduct()">Add Product</button>
            </div>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th>ID</th><th>Name</th><th>Category</th><th>SKU</th><th>Barcode</th><th>Warehouse</th><th>Shelf</th><th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    ${products.map(p => `
                        <tr>
                            <td>${p.id}</td>
                            <td>${p.name}</td>
                            <td>${p.category ? p.category.name : 'N/A'}</td>
                            <td>${p.sku}</td>
                            <td>${p.barcode}</td>
                            <td>${p.warehouseQuantity}</td>
                            <td>${p.shelfQuantity}</td>
                            <td>
                                <button class="btn btn-info btn-sm" onclick="showBatchManager(${p.id}, '${p.name}')">Manage</button>
                                <button class="btn btn-danger btn-sm" onclick="deleteProduct(${p.id})">Delete</button>
                            </td>
                        </tr>
                    `).join('')}
                </tbody>
            </table>
        `;
        content.innerHTML = html;
    } catch (err) {
        content.innerHTML = `<div class="alert alert-danger">Error: ${err.message}</div>`;
    }
}

async function showInventory() {
    const content = document.getElementById('content');
    content.innerHTML = `<h2>Inventory</h2><p>Loading...</p>`;

    try {
        const response = await fetch(`${API_URL}/products`, { headers: getAuthHeaders() });
        if (!response.ok) {
            await showErrorAlert(response, 'Error loading inventory');
            return;
        }
        const products = await response.json();
        const roleId = parseInt(currentUser.roleId);

        let html = `<h2>Inventory Operations</h2><div class="row">`;

        if (roleId === 1 || roleId === 3) {
            html += `
                <div class="col-md-4">
                    <div class="card mb-3"><div class="card-body">
                        <h5>Restock Warehouse</h5>
                        <form id="restockForm">
                            <select id="restockId" class="form-select mb-2" required>${products.map(p => `<option value="${p.id}">${p.name} (W:${p.warehouseQuantity})</option>`).join('')}</select>
                            <input type="number" id="restockQty" class="form-control mb-2" placeholder="Quantity" min="1" required>
                            <input type="number" step="0.01" id="restockPrice" class="form-control mb-2" placeholder="Price" min="0.01" required>
                            <input type="number" id="restockExpire" class="form-control mb-2" placeholder="Days to Expire" min="1" required>
                            <button type="submit" class="btn btn-success w-100">Restock</button>
                        </form>
                    </div></div>
                </div>`;
        }
        if (roleId === 1 || roleId === 2) {
            html += `
                <div class="col-md-4">
                    <div class="card mb-3"><div class="card-body">
                        <h5>Move to Shelf</h5>
                        <form id="moveForm">
                            <select id="moveId" class="form-select mb-2" required>${products.map(p => `<option value="${p.id}">${p.name} (W:${p.warehouseQuantity})</option>`).join('')}</select>
                            <input type="number" id="moveQty" class="form-control mb-2" placeholder="Quantity" min="1" required>
                            <input type="number" step="0.01" id="movePrice" class="form-control mb-2" placeholder="Sell Price" min="0.01" required>
                            <button type="submit" class="btn btn-info w-100">Move</button>
                        </form>
                    </div></div>
                </div>`;
        }
        if (roleId === 1 || roleId === 4) {
            html += `
                <div class="col-md-4">
                    <div class="card mb-3"><div class="card-body">
                        <h5>Sell Product</h5>
                        <form id="sellForm">
                            <select id="sellId" class="form-select mb-2" required>${products.map(p => `<option value="${p.id}">${p.name} (S:${p.shelfQuantity})</option>`).join('')}</select>
                            <input type="number" id="sellQty" class="form-control mb-2" placeholder="Quantity" min="1" required>
                            <select id="sellPayment" class="form-select mb-2"><option value="Cash">Cash</option><option value="Card">Card</option></select>
                            <button type="submit" class="btn btn-danger w-100">Sell</button>
                        </form>
                    </div></div>
                </div>`;
        }

        html += `</div><hr><h4>Current Stock</h4><table class="table table-sm"><thead><tr><th>Product</th><th>Warehouse</th><th>Shelf</th></tr></thead><tbody>${products.map(p => `<tr><td>${p.name}</td><td>${p.warehouseQuantity}</td><td>${p.shelfQuantity}</td></tr>`).join('')}</tbody></table>`;
        content.innerHTML = html;

        if (document.getElementById('restockForm')) {
            document.getElementById('restockForm').addEventListener('submit', async (e) => {
                e.preventDefault();
                const res = await fetch(`${API_URL}/inventory/restock?productId=${document.getElementById('restockId').value}&quantity=${document.getElementById('restockQty').value}&price=${document.getElementById('restockPrice').value}&daysToExpire=${document.getElementById('restockExpire').value}`, { method: 'POST', headers: getAuthHeaders() });
                if (res.ok) { alert('Success'); showInventory(); } else await showErrorAlert(res, 'Restock failed');
            });
        }
        if (document.getElementById('moveForm')) {
            document.getElementById('moveForm').addEventListener('submit', async (e) => {
                e.preventDefault();
                const res = await fetch(`${API_URL}/inventory/move-to-shelf?productId=${document.getElementById('moveId').value}&quantity=${document.getElementById('moveQty').value}&sellPrice=${document.getElementById('movePrice').value}`, { method: 'POST', headers: getAuthHeaders() });
                if (res.ok) { alert('Success'); showInventory(); } else await showErrorAlert(res, 'Move failed');
            });
        }
        if (document.getElementById('sellForm')) {
            document.getElementById('sellForm').addEventListener('submit', async (e) => {
                e.preventDefault();
                const res = await fetch(`${API_URL}/inventory/sell?productId=${document.getElementById('sellId').value}&quantity=${document.getElementById('sellQty').value}&paymentMethod=${document.getElementById('sellPayment').value}`, { method: 'POST', headers: getAuthHeaders() });
                if (res.ok) { alert('Success'); showInventory(); } else await showErrorAlert(res, 'Sale failed');
            });
        }
    } catch (err) { content.innerHTML = `<div class="alert alert-danger">Error: ${err.message}</div>`; }
}

async function showSales(productIdFilter = null) {
    const content = document.getElementById('content');
    content.innerHTML = `<h2>Sales History</h2><p>Loading...</p>`;
    try {
        const prodRes = await fetch(`${API_URL}/products`, { headers: getAuthHeaders() });
        const products = await prodRes.json();
        let url = `${API_URL}/sales${productIdFilter ? `?productId=${productIdFilter}` : ''}`;
        const res = await fetch(url, { headers: getAuthHeaders() });
        if (!res.ok) { await showErrorAlert(res, 'Error loading sales'); return; }
        const sales = await res.json();
        const total = sales.reduce((acc, s) => acc + (s.quantitySold * s.priceSold), 0);

        content.innerHTML = `
            <div class="d-flex justify-content-between mb-4"><h2>Sales History</h2><div class="card bg-success text-white p-2">Total: $${total.toFixed(2)}</div></div>
            <div class="mb-3"><select id="saleFilter" class="form-select w-25" onchange="showSales(this.value)"><option value="">All Products</option>${products.map(p => `<option value="${p.id}" ${productIdFilter==p.id?'selected':''}>${p.name}</option>`).join('')}</select></div>
            <table class="table"><thead><tr><th>Date</th><th>Product</th><th>Qty</th><th>Price</th><th>Total</th></tr></thead>
            <tbody>${sales.map(s => `<tr><td>${new Date(s.shelf?.actionDateTime).toLocaleString()}</td><td>${s.shelf?.product?.name}</td><td>${s.quantitySold}</td><td>$${s.priceSold}</td><td>$${(s.quantitySold*s.priceSold).toFixed(2)}</td></tr>`).join('')}</tbody></table>`;
    } catch (err) { content.innerHTML = `Error: ${err.message}`; }
}

async function showWarehouseLogs(productIdFilter = null) {
    const content = document.getElementById('content');
    content.innerHTML = `<h2>Warehouse Logs</h2><p>Loading...</p>`;
    try {
        const prodRes = await fetch(`${API_URL}/products`, { headers: getAuthHeaders() });
        const products = await prodRes.json();
        let url = `${API_URL}/inventory/warehouse/history${productIdFilter ? `?productId=${productIdFilter}` : ''}`;
        const res = await fetch(url, { headers: getAuthHeaders() });
        if (!res.ok) { await showErrorAlert(res, 'Error loading logs'); return; }
        const history = await res.json();
        content.innerHTML = `
            <h2>Warehouse Logs</h2>
            <div class="mb-3"><select id="whFilter" class="form-select w-25" onchange="showWarehouseLogs(this.value)"><option value="">All Products</option>${products.map(p => `<option value="${p.id}" ${productIdFilter==p.id?'selected':''}>${p.name}</option>`).join('')}</select></div>
            <table class="table"><thead><tr><th>Date</th><th>Product</th><th>Action</th><th>Qty</th><th>Current</th></tr></thead>
            <tbody>${history.map(h => `<tr><td>${new Date(h.actionDateTime).toLocaleString()}</td><td>${h.product?.name}</td><td>${h.actionType?.name}</td><td>${h.quantity}</td><td>${h.currentQuantity}</td></tr>`).join('')}</tbody></table>`;
    } catch (err) { content.innerHTML = `Error: ${err.message}`; }
}

async function showShelfLogs(productIdFilter = null) {
    const content = document.getElementById('content');
    content.innerHTML = `<h2>Shelf Logs</h2><p>Loading...</p>`;
    try {
        const prodRes = await fetch(`${API_URL}/products`, { headers: getAuthHeaders() });
        const products = await prodRes.json();
        let url = `${API_URL}/inventory/shelf/history${productIdFilter ? `?productId=${productIdFilter}` : ''}`;
        const res = await fetch(url, { headers: getAuthHeaders() });
        if (!res.ok) { await showErrorAlert(res, 'Error loading logs'); return; }
        const history = await res.json();
        content.innerHTML = `
            <h2>Shelf Logs</h2>
            <div class="mb-3"><select id="shFilter" class="form-select w-25" onchange="showShelfLogs(this.value)"><option value="">All Products</option>${products.map(p => `<option value="${p.id}" ${productIdFilter==p.id?'selected':''}>${p.name}</option>`).join('')}</select></div>
            <table class="table"><thead><tr><th>Date</th><th>Product</th><th>Action</th><th>Qty</th><th>Current</th></tr></thead>
            <tbody>${history.map(h => `<tr><td>${new Date(h.actionDateTime).toLocaleString()}</td><td>${h.product?.name}</td><td>${h.actionType?.name}</td><td>${h.quantity}</td><td>${h.currentQuantity}</td></tr>`).join('')}</tbody></table>`;
    } catch (err) { content.innerHTML = `Error: ${err.message}`; }
}

function showDashboard() {
    document.getElementById('content').innerHTML = `<h2>Dashboard</h2><p>Welcome back, <b>${currentUser.userName}</b>!</p>`;
}

async function showBatchManager(productId, productName) {
    const modal = new bootstrap.Modal(document.getElementById('appModal'));
    document.getElementById('appModalTitle').textContent = `Batches: ${productName}`;
    document.getElementById('appModalBody').innerHTML = 'Loading...';
    modal.show();
    try {
        const [wRes, sRes] = await Promise.all([
            fetch(`${API_URL}/inventory/warehouse/batches/${productId}`, { headers: getAuthHeaders() }),
            fetch(`${API_URL}/inventory/shelf/batches/${productId}`, { headers: getAuthHeaders() })
        ]);
        const wBatches = await wRes.json();
        const sBatches = await sRes.json();
        let html = '<h5>Warehouse</h5><ul class="list-group mb-3">' + (wBatches.length ? wBatches.map(b => `<li class="list-group-item d-flex justify-content-between">#${b.id} - Qty: ${b.currentQuantity}<button class="btn btn-warning btn-sm" onclick="rejectPrompt('warehouse', ${b.id}, ${productId}, '${productName}')">Reject</button></li>`).join('') : 'No batches') + '</ul>';
        html += '<h5>Shelf</h5><ul class="list-group">' + (sBatches.length ? sBatches.map(b => `<li class="list-group-item d-flex justify-content-between">#${b.id} - Qty: ${b.currentQuantity}<button class="btn btn-warning btn-sm" onclick="rejectPrompt('shelf', ${b.id}, ${productId}, '${productName}')">Reject</button></li>`).join('') : 'No batches') + '</ul>';
        document.getElementById('appModalBody').innerHTML = html;
    } catch (err) { document.getElementById('appModalBody').innerHTML = 'Error loading batches'; }
}

function rejectPrompt(type, id, pId, pName) {
    const reason = prompt('Reason for rejection:');
    if (reason) {
        fetch(`${API_URL}/inventory/reject/${type}`, { method: 'POST', headers: getAuthHeaders(), body: JSON.stringify({ id, reason }) })
            .then(res => res.ok ? (alert('Success'), showBatchManager(pId, pName)) : alert('Failed'));
    }
}

async function deleteProduct(id) {
    if (confirm('Delete product?')) {
        const res = await fetch(`${API_URL}/products/${id}`, { method: 'DELETE', headers: getAuthHeaders() });
        if (res.ok) showProducts(); else await showErrorAlert(res, 'Delete failed');
    }
}

function showAddProduct() {
    document.getElementById('content').innerHTML = `
        <h2>Add Product</h2>
        <form id="addProdForm" class="w-50">
            <input type="text" id="pName" class="form-control mb-2" placeholder="Name" required>
            <input type="text" id="pDesc" class="form-control mb-2" placeholder="Description">
            <select id="pCat" class="form-select mb-2"><option value="1">Fruit</option><option value="2">Vegetable</option><option value="3">Meat</option><option value="4">Dairy</option></select>
            <select id="pSup" class="form-select mb-2"><option value="1">BrandA</option><option value="2">BrandB</option></select>
            <input type="text" id="pSku" class="form-control mb-2" placeholder="SKU">
            <button type="submit" class="btn btn-primary w-100">Save</button>
        </form>`;
    document.getElementById('addProdForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const res = await fetch(`${API_URL}/products`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify({ name: document.getElementById('pName').value, description: document.getElementById('pDesc').value, categoryId: parseInt(document.getElementById('pCat').value), supplierId: parseInt(document.getElementById('pSup').value), sku: document.getElementById('pSku').value, warehouseQuantity: 0, shelfQuantity: 0 })
        });
        if (res.ok) showProducts(); else await showErrorAlert(res, 'Save failed');
    });
}
