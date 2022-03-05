import 'package:cloud_firestore/cloud_firestore.dart';

abstract class GenericModel {
  String? id;
  Timestamp? updatedAt;

  Map<String, dynamic> toJson({bool keepUpdatedAt = true});
}

mixin FirestoreCrudMixin<TItem extends GenericModel> {
  final FirebaseFirestore firestore = FirebaseFirestore.instance;

  Future createItem(TItem item) async {
    try {
      final docItem = firestore.collection(item.runtimeType.toString()).doc();
      item.id = docItem.id;
      await docItem.set(item.toJson());
    } catch (e) {
      Future.error(e);
    }
  }

  Future deleteItem(TItem item) async {
    try {
      final docItem =
          firestore.collection(item.runtimeType.toString()).doc(item.id);
      await docItem.delete();
    } catch (e) {
      Future.error(e);
    }
  }

  Future<bool> checkExist(String collectionId, String documentId) async {
    bool exist = false;
    try {
      await firestore
          .collection(collectionId)
          .doc(documentId)
          .get()
          .then((doc) {
        exist = doc.exists;
      });
    } catch (e) {
      exist = false;
    }
    return exist;
  }

  Future updateItem(TItem item) async {
    try {
      final docItem =
          firestore.collection(item.runtimeType.toString()).doc(item.id);
      await docItem.set(item.toJson());
    } catch (e) {
      Future.error(e);
    }
  }

  Future upsertItem(TItem item) async {
    try {
      if (item.id == null) {
        await createItem(item);
      } else {
        if (await checkExist(item.runtimeType.toString(), item.id!)) {
          await updateItem(item);
        } else {
          await createItem(item);
        }
      }
    } catch (e) {
      Future.error(e);
    }
  }
}
