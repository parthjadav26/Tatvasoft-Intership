import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email = "";
  password = "";
  con_email = "abc@gmail.com";
  con_password = "123";

  Validate_user() {
    if (this.email.trim() === "" || this.password.trim() === "") {
      console.log("Please Enter Email or Password");
    } else {
      if (this.email === this.con_email && this.password === this.con_password) {
        console.log("Login Successful!");
      } else {
        console.log("Invalid Credential");
      }
    }
  }
}
