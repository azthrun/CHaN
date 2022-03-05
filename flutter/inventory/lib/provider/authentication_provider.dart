import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:google_sign_in/google_sign_in.dart';

class AuthenticationProvider {
  final FirebaseAuth _auth = FirebaseAuth.instance;
  Stream<User?> get authStateChange => _auth.authStateChanges();

  Future signInWithGoogle(BuildContext context) async {
    final GoogleSignInAccount? googleUser = await GoogleSignIn().signIn();
    final GoogleSignInAuthentication googleAuth =
        await googleUser!.authentication;
    final credential = GoogleAuthProvider.credential(
      accessToken: googleAuth.accessToken,
      idToken: googleAuth.idToken,
    );

    try {
      await _auth.signInWithCredential(credential);
    } on FirebaseAuthException catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(
            'Error: $e',
            overflow: TextOverflow.ellipsis,
          ),
          backgroundColor: Colors.red[600],
        ),
      );
    }
  }

  Future signOut() async {
    await _auth.signOut();
  }
}
