import React, { useState } from "react";
import "./PaymentForm.css"; // for style

function PaymentForm() {
  const [cardNumber, setCardNumber] = useState("");
  const [expiryDate, setExpiryDate] = useState("");
  const [cvv, setCvv] = useState("");
  const [amount, setAmount] = useState("");
  const [currency, setCurrency] = useState("USD");
  const [message, setMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    const paymentRequestDto = {
      cardNumber,
      expiry,
      cvv,
      amount: parseFloat(amount),
      currency,
    };

    try {
      const response = await fetch("http://localhost:5000/api/payments", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(paymentRequestDto),
      });

      if (response.ok) {
        setMessage("Payment successful!");
      } else {
        const error = await response.text();
        setMessage(`Payment failed: ${error}`);
      }
    } catch (err) {
      setMessage(`Error: ${err.message}`);
    }
};

  return (
    <div className="payment-container">
      <form className="payment-form" onSubmit={handleSubmit}>
        <h2>QuickPay Ödeme</h2>
        <div className="form-group">
          <label>Kart Numarası</label>
          <input
            type="text"
            value={cardNumber}
            onChange={(e) => setCardNumber(e.target.value)}
            placeholder="1234 5678 9012 3456"
            maxLength={19}
          />
        </div>
        <div className="form-group">
          <label>Son Kullanma Tarihi</label>
          <input
            type="text"
            value={expiryDate}
            onChange={(e) => setExpiryDate(e.target.value)}
            placeholder="MM/YY"
            maxLength={5}
          />
        </div>
        <div className="form-group">
          <label>CVV</label>
          <input
            type="password"
            value={cvv}
            onChange={(e) => setCvv(e.target.value)}
            placeholder="123"
            maxLength={3}
          />
        </div>
        <div className="form-group">
          <label>Tutar</label>
          <input
            type="number"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            placeholder="0.00"
            step="0.01"
          />
        </div>
        <div className="form-group">
          <label>Para Birimi</label>
          <select
            value={currency}
            onChange={(e) => setCurrency(e.target.value)}
          >
            <option value="USD">USD</option>
            <option value="EUR">EUR</option>
            <option value="TRY">TRY</option>
          </select>
        </div>
        <button type="submit">Ödeme Yap</button>
        {message && <p className="message">{message}</p>}
      </form>
    </div>
  );
}

export default PaymentForm;
