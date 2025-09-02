import React, { useState } from "react";
import "./PaymentForm.css"; // for style

function PaymentForm() {
  const [cardNumber, setCardNumber] = useState("");
  const [expiryDate, setExpiryDate] = useState("");
  const [cvv, setCvv] = useState("");
  const [amount, setAmount] = useState("");
  const [currency, setCurrency] = useState("USD");
  const [message, setMessage] = useState("");

  // cardNumber mask
   const formatCardNumber = (value) => {
    return value
      .replace(/\D/g, "")
      .replace(/(.{4})/g, "$1 ")
      .trim();
  };

  // Expiry date mask
  const formatExpiry = (value) => {
    return value
      .replace(/\D/g, "")
      .replace(/^(\d{2})(\d{1,2})?$/, "$1/$2")
      .slice(0, 5);
  };

  const formatCvv = (value) => value.replace(/\D/g, "").slice(0, 3);

  const validateInputs = () => {
    if (!/^\d{16}$/.test(cardNumber.replace(/\s+/g, ""))) {
      setMessage("Kart numarası 16 hane olmalı.");
      return false;
    }

    if (!/^\d{2}\/\d{2}$/.test(expiryDate)) {
      setMessage("Son kullanım tarihi MM/YY veya MM/YYYY formatında olmalı.");
      return false;
    }

    if (!/^\d{3}$/.test(cvv)) {
      setMessage("CVV 3 hane olmalı.");
      return false;
    }

    if (parseFloat(amount) <= 0 || isNaN(parseFloat(amount))) {
      setMessage("Tutar pozitif bir sayı olmalı.");
      return false;
    }

    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateInputs()) return; // validation failed

    const paymentRequestDto = {
      cardNumber,
      expiry : expiryDate,
      cvv,
      amount: parseFloat(amount),
      currency,
    };

    try {
      const request = await fetch("http://localhost:5000/api/payments", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(paymentRequestDto),
      });

      if (request.ok) {
        setMessage("Payment successful!");
      } else {
        const error = await request.text();
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
            onChange={(e) => setCardNumber(formatCardNumber(e.target.value))}
            placeholder="1234 5678 9012 3456"
            maxLength={19}
          />
        </div>

        <div className="form-group">
          <label>Son Kullanma Tarihi</label>
          <input
            type="text"
            value={expiryDate}
            onChange={(e) => setExpiryDate(formatExpiry(e.target.value))}
            placeholder="MM/YY"
            maxLength={5}
          />
        </div>

        <div className="form-group">
          <label>CVV</label>
          <input
            type="password"
            value={cvv}
            onChange={(e) => setCvv(formatCvv(e.target.value))}
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
